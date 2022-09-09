module JuliaPlugins.ColorSelectionCommand

open System
open System.Text.RegularExpressions
open Discord
open SharedInterfaces

let max_color_roles = 25

let named_colors =
    Map [ ("blue", Color.Blue)
          ("dark blue", Color.DarkBlue)
          ("darker grey", Color.DarkerGrey)
          ("dark green", Color.DarkGreen)
          ("dark grey", Color.DarkGrey)
          ("dark magenta", Color.DarkMagenta)
          ("dark orange", Color.DarkOrange)
          ("dark purple", Color.DarkPurple)
          ("dark red", Color.DarkRed)
          ("dark teal", Color.DarkTeal)
          ("default", Color.Default)
          ("gold", Color.Gold)
          ("green", Color.Green)
          ("lighter grey", Color.LighterGrey)
          ("light grey", Color.LightGrey)
          ("light orange", Color.LightOrange)
          ("magenta", Color.Magenta)
          ("orange", Color.Orange)
          ("purple", Color.Purple)
          ("red", Color.Red)
          ("teal", Color.Teal) ]

type ListColorsCommand() =
    member this.Name = "list_colors"

    interface ICommand with
        member this.Build() =
            SlashCommandBuilder()
                .WithName(this.Name)
                .WithDescription("List named colors used by the /color command")
                .Build()

        member this.Execute(command, _client) =
            let color_names =
                Map.toSeq named_colors
                |> Seq.map (fun (name, color) -> $"- %s{name}: %s{color.ToString()}")
                |> String.concat "\n"

            command.RespondAsync("Available colors:\n" + color_names)

        member this.Name = this.Name

let hex_color_pattern =
    Regex("^#[0-9a-zA-Z]{6}$")

let color_role_name_pattern =
    Regex("^Color (#[0-9A-F]{6})$")

let is_color_role (role: IRole) =
    color_role_name_pattern.IsMatch role.Name


type ColorCommand() =
    member this.Name = "color"

    interface ICommand with
        member this.Build() =
            SlashCommandBuilder()
                .WithName(this.Name)
                .WithDescription("Set name role color")
                .AddOption(
                    SlashCommandOptionBuilder()
                        .WithName("color")
                        .WithDescription("Color hex code or name")
                        .WithRequired(true)
                        .WithType(ApplicationCommandOptionType.String)
                )
                .Build()

        member this.Execute(command, client) =
            task {
                try
                    let options =
                        command.Data.Options
                        |> Seq.map (fun opt -> (opt.Name, opt))
                        |> Map.ofSeq

                    let color_string =
                        options["color"].Value :?> string

                    let color =
                        if hex_color_pattern.IsMatch color_string then
                            let hex_string =
                                color_string.Substring(1, 6)

                            Color(UInt32.Parse(hex_string, System.Globalization.NumberStyles.HexNumber))
                        else
                            match Map.tryFind (color_string.ToLower()) named_colors with
                            | Some color -> color
                            | None -> failwith $"No color matching '%s{color_string}' was found."

                    let guild =
                        match Option.map client.GetGuild (Option.ofNullable command.GuildId) with
                        | Some guild -> guild
                        | None -> failwith "This command must be sent from a guild."

                    let color_roles =
                        guild.Roles
                        |> Seq.filter is_color_role
                        |> Seq.map (fun role ->
                            ((color_role_name_pattern.Match role.Name).Groups[1]
                                .Value,
                             role))
                        |> Map.ofSeq

                    let! new_color_role =
                        match Map.tryFind (color.ToString()) color_roles with
                        | Some color_role -> task { return color_role :> IRole }
                        | None ->
                            task {
                                if color_roles.Count > max_color_roles then
                                    failwith "Too many color roles exist, please use a different color."

                                let! role =
                                    guild.CreateRoleAsync(
                                        $"Color %s{color.ToString()}",
                                        Option.toNullable None,
                                        Option.toNullable (Some color),
                                        false,
                                        false
                                    )

                                return role :> IRole
                            }

                    let guild_user =
                        guild.GetUser command.User.Id

                    let user_color_roles =
                        guild_user.Roles
                        |> Seq.filter is_color_role
                        |> Seq.map (fun role -> (role.Id, role))
                        |> Map.ofSeq

                    let already_has_role =
                        Map.containsKey new_color_role.Id user_color_roles

                    let roles_to_remove =
                        Map.remove new_color_role.Id user_color_roles
                        |> Map.values

                    do! guild_user.RemoveRolesAsync(Seq.map (fun role -> role :> IRole) roles_to_remove)

                    // Delete any color roles that don't have members anymore
                    for role in roles_to_remove do
                        if Seq.isEmpty (Seq.filter ((<>) guild_user) role.Members) then
                            do! role.DeleteAsync()

                    if not already_has_role then
                        do! guild_user.AddRoleAsync new_color_role

                    return! command.RespondAsync $"Color has been set to %s{color.ToString()}"
                with
                | Failure msg -> return! command.RespondAsync $"Error: %s{msg}"
            }

        member this.Name = this.Name
