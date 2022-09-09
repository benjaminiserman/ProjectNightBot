namespace JuliaPlugins

open Discord
open SharedInterfaces

module Commands =
    type JuliaGreetingCommand() =
        
        interface ICommand with
            member this.Name = "greet_julia"
            
            member this.Build() =
                SlashCommandBuilder()
                    .WithName((this :> ICommand).Name)
                    .WithDescription("Julia says hi!")
                    .Build()

            member this.Execute command =
                task {
                    return! command.RespondAsync "Hello world! - Julia"
                }
        
    