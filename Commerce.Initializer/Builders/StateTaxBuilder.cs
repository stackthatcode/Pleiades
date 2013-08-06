using System;
using System.Transactions;
using Commerce.Persist.Database;
using Pleiades.Injection;
using Pleiades.Data;
using Commerce.Persist.Concrete;
using Commerce.Persist.Model.Billing;

namespace Commerce.Initializer.Builders
{
    public class StateTaxBuilder
    {
        public static void Populate(IContainerAdapter ServiceLocator)
        {
            using (var tx = new TransactionScope())
            {
                Console.WriteLine("Create the default State Taxes");
                var context = ServiceLocator.Resolve<PushMarketContext>();

                context.StateTaxes.Add(new StateTax { 
                    Name = "Alabama",
                    Abbreviation = "AL",
                    TaxRate = 4.0m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Alaska",
                    Abbreviation = "AK",
                    TaxRate = 0.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Arizona",
                    Abbreviation = "AZ",
                    TaxRate = 6.6m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Arkansas",
                    Abbreviation = "AR",
                    TaxRate = 6.0m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "California",
                    Abbreviation = "CA",
                    TaxRate = 7.5m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Colorado",
                    Abbreviation = "CO",
                    TaxRate = 2.9m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Connecticut",
                    Abbreviation = "CT",
                    TaxRate = 6.35m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Delaware",
                    Abbreviation = "DE",
                    TaxRate = 0.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "District Of Columbia",
                    Abbreviation = "DC",
                    TaxRate = 6.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Florida",
                    Abbreviation = "FL",
                    TaxRate = 6.35m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Georgia",
                    Abbreviation = "GA",
                    TaxRate = 4.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Hawaii",
                    Abbreviation = "HI",
                    TaxRate = 4.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Idaho",
                    Abbreviation = "ID",
                    TaxRate = 6.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Illinois",
                    Abbreviation = "IL",
                    TaxRate = 6.25m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Indiana",
                    Abbreviation = "IN",
                    TaxRate = 7.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Iowa",
                    Abbreviation = "IA",
                    TaxRate = 6.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Kansas",
                    Abbreviation = "KS",
                    TaxRate = 6.30m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Kentucky",
                    Abbreviation = "KY",
                    TaxRate = 6.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Louisiana",
                    Abbreviation = "LA",
                    TaxRate = 4.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Maine",
                    Abbreviation = "ME",
                    TaxRate = 5.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Maryland",
                    Abbreviation = "MD",
                    TaxRate = 6.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Massachusetts",
                    Abbreviation = "MA",
                    TaxRate = 6.25m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Michigan",
                    Abbreviation = "MI",
                    TaxRate = 6.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Minnesota",
                    Abbreviation = "MN",
                    TaxRate = 6.875m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Mississippi",
                    Abbreviation = "MS",
                    TaxRate = 7.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Missouri",
                    Abbreviation = "MO",
                    TaxRate = 4.225m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Montana",
                    Abbreviation = "MT",
                    TaxRate = 0.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Nebraska",
                    Abbreviation = "NE",
                    TaxRate = 5.50m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Nevada",
                    Abbreviation = "NV",
                    TaxRate = 6.85m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "New Hampshire",
                    Abbreviation = "NH",
                    TaxRate = 0.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "New Jersey",
                    Abbreviation = "NJ",
                    TaxRate = 7.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "New Mexico",
                    Abbreviation = "NM",
                    TaxRate = 5.125m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "New York",
                    Abbreviation = "NY",
                    TaxRate = 4.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "North Carolina",
                    Abbreviation = "NC",
                    TaxRate = 4.75m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "North Dakota",
                    Abbreviation = "ND",
                    TaxRate = 5.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Ohio",
                    Abbreviation = "OH",
                    TaxRate = 5.50m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Oklahoma",
                    Abbreviation = "OK",
                    TaxRate = 4.50m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Oregon",
                    Abbreviation = "OR",
                    TaxRate = 0.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Pennsylvania",
                    Abbreviation = "PA",
                    TaxRate = 6.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Rhode Island",
                    Abbreviation = "RI",
                    TaxRate = 7.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "South Carolina",
                    Abbreviation = "SC",
                    TaxRate = 6.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "South Dakota",
                    Abbreviation = "SD",
                    TaxRate = 4.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Tennessee",
                    Abbreviation = "TN",
                    TaxRate = 7.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Texas",
                    Abbreviation = "TX",
                    TaxRate = 6.25m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Utah",
                    Abbreviation = "UT",
                    TaxRate = 4.70m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Vermont",
                    Abbreviation = "VT",
                    TaxRate = 6.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Virginia",
                    Abbreviation = "VA",
                    TaxRate = 4.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Washington",
                    Abbreviation = "WA",
                    TaxRate = 6.50m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "West Virginia",
                    Abbreviation = "WV",
                    TaxRate = 6.00m,
                });
                context.StateTaxes.Add(new StateTax { 
                    Name = "Wisconsin",
                    Abbreviation = "WI",
                    TaxRate = 5.00m,
                });
                context.StateTaxes.Add(new StateTax
                {
                    Name = "Wyoming",
                    Abbreviation = "WY",
                    TaxRate = 4.00m,
                });

                context.SaveChanges();
                tx.Complete();
            }
        }
    }
}
