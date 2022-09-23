module JuliaPlugins.MandoMondayCommand

open Discord
open SharedInterfaces

type MandoMondayCommand() =
    member this.Name = "mandomonday"

    interface ICommand with
        member this.Name = this.Name

        member this.Build() =
            SlashCommandBuilder()
                .WithName(this.Name)
                .WithDescription("Mando Monday")
                .Build()

        member this.Execute(command, _client) =
            command.RespondAsync "<@87229746663931904>\nhttps://youtu.be/4Y0Z1GxtfkU"

type NekoCommand() =
    member this.Name = "neko"

    interface ICommand with
        member this.Name = this.Name

        member this.Build() =
            SlashCommandBuilder()
                .WithName(this.Name)
                .WithDescription("Catboy pictures")
                .Build()

        member this.Execute(command, _client) =
            let images =
                [| "https://cdn.discordapp.com/attachments/272388063643303938/1022660581083316224/neko0.png"
                   "https://cdn.discordapp.com/attachments/272388063643303938/1022660600632987668/neko1.png"
                   "https://cdn.discordapp.com/attachments/272388063643303938/1022660993840590878/neko2.png"
                   "https://cdn.discordapp.com/attachments/272388063643303938/1022660559499427871/neko3.png"
                   "https://cdn.discordapp.com/attachments/272388063643303938/1022660537416421456/neko4.png" |]

            command.RespondAsync images[System.Random.Shared.Next(5)]
