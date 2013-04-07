using System;
using System.Transactions;
using Pleiades.Injection;
using Pleiades.Data;
using Commerce.Persist.Concrete;
using Commerce.Persist.Model.Billing;

namespace Commerce.Initializer.Builders
{
    public class StateTaxBuilder
    {
        public static void EmptyAndRepopulate(IContainerAdapter ServiceLocator)
        {
            using (var tx = new TransactionScope())
            {
                Console.WriteLine("Create the default State Taxes");
                var context = ServiceLocator.Resolve<PleiadesContext>();

                context.StateTaxes.Add(new StateTax { 
                    Name = "Alabama",
                    Abbreviation = "AL",
                    Tax = 4.0m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Alaska",
                    Abbreviation = "AK",
                    Tax = 0.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Arizona",
                    Abbreviation = "AZ",
                    Tax = 6.6m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Arkansas",
                    Abbreviation = "AR",
                    Tax = 6.0m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "California",
                    Abbreviation = "CA",
                    Tax = 7.5m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Colorado",
                    Abbreviation = "CO",
                    Tax = 2.9m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Connecticut",
                    Abbreviation = "CT",
                    Tax = 6.35m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Delaware",
                    Abbreviation = "DE",
                    Tax = 0.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "District Of Columbia",
                    Abbreviation = "DC",
                    Tax = 6.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Florida",
                    Abbreviation = "FL",
                    Tax = 6.35m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Georgia",
                    Abbreviation = "GA",
                    Tax = 4.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Hawaii",
                    Abbreviation = "HI",
                    Tax = 4.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Idaho",
                    Abbreviation = "ID",
                    Tax = 6.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Illinois",
                    Abbreviation = "IL",
                    Tax = 6.25m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Indiana",
                    Abbreviation = "IN",
                    Tax = 7.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Iowa",
                    Abbreviation = "IA",
                    Tax = 6.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Kansas",
                    Abbreviation = "KS",
                    Tax = 6.30m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Kentucky",
                    Abbreviation = "KY",
                    Tax = 6.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Louisiana",
                    Abbreviation = "LA",
                    Tax = 4.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Maine",
                    Abbreviation = "ME",
                    Tax = 5.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Maryland",
                    Abbreviation = "MD",
                    Tax = 6.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Massachusetts",
                    Abbreviation = "MA",
                    Tax = 6.25m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Michigan",
                    Abbreviation = "MI",
                    Tax = 6.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Minnesota",
                    Abbreviation = "MN",
                    Tax = 6.875m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Mississippi",
                    Abbreviation = "MS",
                    Tax = 7.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Missouri",
                    Abbreviation = "MO",
                    Tax = 4.225m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Montana",
                    Abbreviation = "MT",
                    Tax = 0.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Nebraska",
                    Abbreviation = "NE",
                    Tax = 5.50m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Nevada",
                    Abbreviation = "NV",
                    Tax = 6.85m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "New Hampshire",
                    Abbreviation = "NH",
                    Tax = 0.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "New Jersey",
                    Abbreviation = "NJ",
                    Tax = 7.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "New Mexico",
                    Abbreviation = "NM",
                    Tax = 5.125m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "New York",
                    Abbreviation = "NY",
                    Tax = 4.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "North Carolina",
                    Abbreviation = "NC",
                    Tax = 4.75m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "North Dakota",
                    Abbreviation = "ND",
                    Tax = 5.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Ohio",
                    Abbreviation = "OH",
                    Tax = 5.50m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Oklahoma",
                    Abbreviation = "OK",
                    Tax = 4.50m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Oregon",
                    Abbreviation = "OR",
                    Tax = 0.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Pennsylvania",
                    Abbreviation = "PA",
                    Tax = 6.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Rhode Island",
                    Abbreviation = "RI",
                    Tax = 7.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "South Carolina",
                    Abbreviation = "SC",
                    Tax = 6.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "South Dakota",
                    Abbreviation = "SD",
                    Tax = 4.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Tennessee",
                    Abbreviation = "TN",
                    Tax = 7.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Texas",
                    Abbreviation = "TX",
                    Tax = 6.25m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Utah",
                    Abbreviation = "UT",
                    Tax = 4.70m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Vermont",
                    Abbreviation = "VT",
                    Tax = 6.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Virginia",
                    Abbreviation = "VA",
                    Tax = 4.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Washington",
                    Abbreviation = "WA",
                    Tax = 6.50m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "West Virginia",
                    Abbreviation = "WV",
                    Tax = 6.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Wisconsin",
                    Abbreviation = "WI",
                    Tax = 5.00m,
                });
                context.StateTaxes.Add(new StateTax
                {
                    Name = "Wyoming",
                    Abbreviation = "WY",
                    Tax = 4.00m,
                });

                context.SaveChanges();
                tx.Complete();
            }
        }
    }
}
