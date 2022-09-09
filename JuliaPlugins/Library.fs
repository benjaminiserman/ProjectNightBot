namespace JuliaPlugins

open Discord
open SharedInterfaces

type JuliaGreetingCommand() =

    member this.Name = "greet_julia"

    interface ICommand with
        member this.Name = this.Name

        member this.Build() =
            SlashCommandBuilder()
                .WithName(this.Name)
                .WithDescription("Julia says hi!")
                .Build()

        member this.Execute(command, _client) =
            command.RespondAsync "Hello world! - Julia"
