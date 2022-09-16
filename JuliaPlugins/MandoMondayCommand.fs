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
        