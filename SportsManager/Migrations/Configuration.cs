namespace SportsManager.Migrations
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SportsManager.Models.SportsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "SportsManager.Models.SportsContext";
        }

        protected override void Seed(SportsManager.Models.SportsContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var games = new List<Game>
            {
                new Game {gameCode="CYCL123", gameName="Cycling", gameDescription="Some nonsense", gameDurationInMins=60, gameBookletPath="/Upload/Rules/Doc1.pdf"},
                new Game {gameCode="RUNN123", gameName="Running", gameDescription="Some other nonsense", gameDurationInMins=18, gameBookletPath="/Upload/Rules/Doc2.pdf"},
                new Game {gameCode="SWIM123", gameName="Swimming", gameDescription="Some minor nonsense", gameDurationInMins=25, gameBookletPath="/Upload/Rules/Doc3.pdf"},
                new Game {gameCode="JUMP123", gameName="High Jump", gameDescription="Some arbitrary nonsense", gameDurationInMins=30, gameBookletPath="/Upload/Rules/Doc4.pdf"},
            };

            games.ForEach(g => context.Games.AddOrUpdate(p => p.gameCode, g));
            context.SaveChanges();

            var competitors = new List<Competitor>
            {
                new Competitor {competitorSalutation="Mrs",competitorName="Ramona Smith",
                    competitorDoB =DateTime.Parse("1978/06/10"), competitorEmail ="ramona.smith24@example.com",
                    competitorCountry ="Canada", competitorDescription ="More silly nonsense",
                    competitorGender ="Female", competitorPhone ="+14031231234",
                    competitorWebsite = "http://somesite.com", competitorPhotoPath ="/Upload/Photos/face2.jpg"},
                new Competitor {competitorSalutation="Mr",competitorName="Stephen Robertson",
                    competitorDoB =DateTime.Parse("1984/07/12"), competitorEmail="stephen.robertson@example.com",
                    competitorCountry ="United States", competitorDescription="More nilly sonsense",
                    competitorGender ="Male", competitorPhone="+12021231234",
                    competitorWebsite = "http://somesite.com", competitorPhotoPath="/Upload/Photos/face1.jpg"},
                new Competitor {competitorSalutation="Mr",competitorName="Rob Johnson",
                    competitorDoB =DateTime.Parse("1988/01/12"), competitorEmail="rob.johnson@gmail.com",
                    competitorCountry ="Australia", competitorDescription="A bit less silly nonsense",
                    competitorGender ="Male", competitorPhone="+61409876123",
                    competitorWebsite = "http://someothersite.com", competitorPhotoPath="/Upload/Photos/face3.jpg"},
                new Competitor {competitorSalutation="Mr",competitorName="Job Rohnson",
                    competitorDoB =DateTime.Parse("1910/04/20"), competitorEmail="job.rohnson@gmail.com",
                    competitorCountry ="United Kingdom", competitorDescription="A bit more silly nonsense",
                    competitorGender ="Male", competitorPhone="+444098761231",
                    competitorWebsite = "http://someothersiteagain.com", competitorPhotoPath="/Upload/Photos/face4.jpg"},
                new Competitor {competitorSalutation="Miss",competitorName="Donald Trump",
                    competitorDoB =DateTime.Parse("1985/10/16"), competitorEmail="donald.trump@gmail.com",
                    competitorCountry ="United States", competitorDescription="A LOT silly nonsense",
                    competitorGender ="Female", competitorPhone="+15559876123",
                    competitorWebsite = "http://likeseriously.com", competitorPhotoPath="/Upload/Photos/face7.jpg"},
                new Competitor {competitorSalutation="Mr",competitorName="John Smithington",
                    competitorDoB =DateTime.Parse("1966/11/02"), competitorEmail="john.smithington@gmail.com",
                    competitorCountry ="Australia", competitorDescription="A jolly bit of silly nonsense",
                    competitorGender ="Male", competitorPhone="+61409666111",
                    competitorWebsite = "http://abcdefgh.com", competitorPhotoPath="/Upload/Photos/face5.jpg"},
                new Competitor {competitorSalutation="Mr",competitorName="Okidoki MrPokey",
                    competitorDoB =DateTime.Parse("1977/11/02"), competitorEmail="okidoki.mrpokey@gmail.com",
                    competitorCountry ="China", competitorDescription="Something about nonsense here",
                    competitorGender ="Male", competitorPhone="+862087655678",
                    competitorWebsite = "http://ALLTHEWEBSITES.com", competitorPhotoPath="/Upload/Photos/face6.jpg"},
                new Competitor {competitorSalutation="Miss",competitorName="Ohme Gawd",
                    competitorDoB =DateTime.Parse("1992/08/01"), competitorEmail="ohme.gawd@gmail.com",
                    competitorCountry ="China", competitorDescription="More of the same really",
                    competitorGender ="Female", competitorPhone="+862012340987",
                    competitorWebsite = "http://anotherwebsite.com", competitorPhotoPath="/Upload/Photos/face8.jpg"},
                new Competitor {competitorSalutation="Dr",competitorName="Whats Hisface",
                    competitorDoB =DateTime.Parse("1924/03/06"), competitorEmail ="whats.hisface@example.com",
                    competitorCountry ="Canada", competitorDescription ="Something something nonsense",
                    competitorGender ="Male", competitorPhone ="+14034321567",
                    competitorWebsite = "http://morewebsites.com", competitorPhotoPath ="/Upload/Photos/face9.jpg"},
                new Competitor {competitorSalutation="Dr",competitorName="Whats Hisface",
                    competitorDoB =DateTime.Parse("1924/03/06"), competitorEmail ="whats.hisface@example.com",
                    competitorCountry ="Canada", competitorDescription ="Something something nonsense",
                    competitorGender ="Male", competitorPhone ="+14034321567",
                    competitorWebsite = "http://morewebsites.com", competitorPhotoPath ="/Upload/Photos/face10.jpg"}
           };

            List<Game> gamesList = context.Games.ToList();

            competitors[0].Games.Add(gamesList[0]); competitors[0].Games.Add(gamesList[1]); competitors[0].Games.Add(gamesList[2]);
            competitors[1].Games.Add(gamesList[0]); competitors[1].Games.Add(gamesList[1]); competitors[1].Games.Add(gamesList[2]);
            competitors[2].Games.Add(gamesList[0]); competitors[2].Games.Add(gamesList[1]); competitors[2].Games.Add(gamesList[2]);
            competitors[3].Games.Add(gamesList[0]); competitors[3].Games.Add(gamesList[1]); competitors[3].Games.Add(gamesList[2]);
            competitors[4].Games.Add(gamesList[0]); competitors[4].Games.Add(gamesList[1]); competitors[4].Games.Add(gamesList[2]);
            competitors[5].Games.Add(gamesList[0]); competitors[5].Games.Add(gamesList[1]); competitors[5].Games.Add(gamesList[2]);
            competitors[6].Games.Add(gamesList[0]); competitors[6].Games.Add(gamesList[1]); competitors[6].Games.Add(gamesList[2]);
            competitors[7].Games.Add(gamesList[0]); competitors[7].Games.Add(gamesList[1]); competitors[7].Games.Add(gamesList[2]);
            competitors[8].Games.Add(gamesList[0]); competitors[8].Games.Add(gamesList[1]); competitors[8].Games.Add(gamesList[2]);
            competitors[9].Games.Add(gamesList[0]); competitors[9].Games.Add(gamesList[1]); competitors[9].Games.Add(gamesList[2]);

            competitors.ForEach(c => context.Competitors.AddOrUpdate(p => p.competitorName, c));
            context.SaveChanges();

            var tags = new List<Tag>()
            {
                new Tag {tagString="cycling"},
                new Tag {tagString="swimming"},
                new Tag {tagString="running"},
                new Tag {tagString="gold"},
                new Tag {tagString="world record"}
            };

            tags.ForEach(t => context.Tags.Add(t));
            context.SaveChanges();

            List<Tag> tagList = context.Tags.ToList();

            var photos = new List<Photo>()
            {
                new Photo {photoPath="/Upload/Photos/event1.png"},
                new Photo {photoPath="/Upload/Photos/event2.png"},
                new Photo {photoPath="/Upload/Photos/event3.png"},
                new Photo {photoPath="/Upload/Photos/event4.png"},
                new Photo {photoPath="/Upload/Photos/event5.png"}
            };

            photos[0].Tags.Add(tagList[0]); photos[0].Tags.Add(tagList[3]);
            photos[1].Tags.Add(tagList[0]); photos[2].Tags.Add(tagList[1]);
            photos[2].Tags.Add(tagList[4]); photos[3].Tags.Add(tagList[1]);
            photos[4].Tags.Add(tagList[2]); photos[4].Tags.Add(tagList[3]);

            var events = new List<Event>()
            {
                new Event {gameID = 1, featureEvent=true,
                    eventVenue ="ECU", eventDate=DateTime.Parse("2016/10/12"),
                    eventStartTime =DateTime.Parse("18:00"), eventEndTime =DateTime.Parse("19:00"),
                    eventDescription="A cycling event", worldRecord=false},
                new Event {gameID = 2, featureEvent=false,
                    eventVenue ="ECU", eventDate=DateTime.Parse("2016/10/12"),
                    eventStartTime =DateTime.Parse("18:00"), eventEndTime =DateTime.Parse("19:00"),
                    eventDescription="A cycling event", worldRecord=true},
                new Event {gameID = 3, featureEvent=false,
                    eventVenue ="ECU", eventDate=DateTime.Parse("2016/08/01"),
                    eventStartTime =DateTime.Parse("08:00"), eventEndTime =DateTime.Parse("08:25"),
                    eventDescription="A swimming event", worldRecord=true}
            };

            events[0].Photos.Add(photos[0]); events[0].Photos.Add(photos[1]);
            events[1].Photos.Add(photos[2]); events[1].Photos.Add(photos[3]);
            events[2].Photos.Add(photos[4]);

            events.ForEach(e => context.Events.Add(e));
            context.SaveChanges();

            var eventCompetitors = new List<EventCompetitor>()
            {
                new EventCompetitor {competitorPosition=1, competitorMedal="Gold"}, // Event 1
                new EventCompetitor {competitorPosition=2, competitorMedal="Silver"}, // Event 1
                new EventCompetitor {competitorPosition=3, competitorMedal="Bronze"}, // Event 1
                new EventCompetitor {competitorPosition=4, competitorMedal="None"}, // Event 1
                new EventCompetitor {competitorPosition=5, competitorMedal="None"}, // Event 1
                new EventCompetitor {competitorPosition=6, competitorMedal="None"}, // Event 1
                new EventCompetitor {competitorPosition=1, competitorMedal="Gold"}, // Event 2
                new EventCompetitor {competitorPosition=2, competitorMedal="Silver"}, // Event 2
                new EventCompetitor {competitorPosition=3, competitorMedal="Bronze"}, // Event 2
                new EventCompetitor {competitorPosition=4, competitorMedal="None"}, // Event 2
                new EventCompetitor {competitorPosition=5, competitorMedal="None"}, // Event 2
                new EventCompetitor {competitorPosition=6, competitorMedal="None"}, // Event 2
                new EventCompetitor {competitorPosition=1, competitorMedal="Gold"}, // Event 3
                new EventCompetitor {competitorPosition=2, competitorMedal="Silver"}, // Event 3
                new EventCompetitor {competitorPosition=3, competitorMedal="Bronze"}, // Event 3
                new EventCompetitor {competitorPosition=4, competitorMedal="None"}, // Event 3
                new EventCompetitor {competitorPosition=5, competitorMedal="None"}, // Event 3
                new EventCompetitor {competitorPosition=6, competitorMedal="None"} // Event 3
            };

            List<Event> eventList = context.Events.ToList();

            eventCompetitors[0].Event = eventList[0];
            eventCompetitors[1].Event = eventList[0];
            eventCompetitors[2].Event = eventList[0];
            eventCompetitors[3].Event = eventList[0];
            eventCompetitors[4].Event = eventList[0];
            eventCompetitors[5].Event = eventList[0];
            eventCompetitors[6].Event = eventList[1];
            eventCompetitors[7].Event = eventList[1];
            eventCompetitors[8].Event = eventList[1];
            eventCompetitors[9].Event = eventList[1];
            eventCompetitors[10].Event = eventList[1];
            eventCompetitors[11].Event = eventList[1];
            eventCompetitors[12].Event = eventList[2];
            eventCompetitors[13].Event = eventList[2];
            eventCompetitors[14].Event = eventList[2];
            eventCompetitors[15].Event = eventList[2];
            eventCompetitors[16].Event = eventList[2];
            eventCompetitors[17].Event = eventList[2];

            List<Competitor> competitorList = context.Competitors.ToList();

            eventCompetitors[0].Competitor = competitorList[0];
            eventCompetitors[1].Competitor = competitorList[2];
            eventCompetitors[2].Competitor = competitorList[4];
            eventCompetitors[3].Competitor = competitorList[5];
            eventCompetitors[4].Competitor = competitorList[7];
            eventCompetitors[5].Competitor = competitorList[9];
            eventCompetitors[6].Competitor = competitorList[1];
            eventCompetitors[7].Competitor = competitorList[3];
            eventCompetitors[8].Competitor = competitorList[5];
            eventCompetitors[9].Competitor = competitorList[6];
            eventCompetitors[10].Competitor = competitorList[8];
            eventCompetitors[11].Competitor = competitorList[9];
            eventCompetitors[12].Competitor = competitorList[0];
            eventCompetitors[13].Competitor = competitorList[2];
            eventCompetitors[14].Competitor = competitorList[3];
            eventCompetitors[15].Competitor = competitorList[6];
            eventCompetitors[16].Competitor = competitorList[7];
            eventCompetitors[17].Competitor = competitorList[8];

            eventCompetitors.ForEach(ec => context.EventCompetitors.Add(ec));
            context.SaveChanges();

        }
    }
}
