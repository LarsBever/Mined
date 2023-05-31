using Mined.Data;
using System;

namespace Mined
{
    class program
    {
        private static MinedDbContext context = new MinedDbContext();

        //Toe te voegen aan de Score Repo:
        // - iets dat er voor zorgt dat enkel de 10 bovenste scores worden getoont
        // - iets voor het inserten van gegevens van de speler en het bijhouden van de score
        // - iets voor de admin om de tabel evt aan te passen

        //Hieronder Select statement voor het opzoeken van de scores (alleen de Top 10!)

        //Basic Insert statement to retrieve the top 10 scores
        public async List<Scores> Get()
        {
            var score = new Score { Id = ,
                                    Nickname = "",
                                    NumberOfMistakes = // aantal verkeerd geraden uxo's
                                    UxoMistakes = //hier de UxoNamen (string) die verkeerd zijn geraden
                                    PlayerScore = //totale score vd speler
            };
            await context.Scores.AddAsync(score);

            await context.SaveChangesAsync();
        }

        //




    }

}