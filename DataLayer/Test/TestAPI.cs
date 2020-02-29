using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using System.Net.Http;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TestModules
{
    public class TestAPI
    {
        
        public static Dictionary<string, string> JsonResponses = new Dictionary<string, string>();
        
        public static void InitJsons() {
            JsonResponses.Add("AddReturnAddress1", @"	{
                                        ""status"": 1,
                                        ""message"": ""Successfully Added Return Address"",
                                        ""data"": [     
                                            {
                                                ""IsDefault"": true,
						                        ""ReturnAddressId"": 1,
                                                ""FullAddress"": ""Full Return Address 1""
					                        }],
                                        ""error"": []
                                    }");
            JsonResponses.Add("AddReturnAddress2", @"{
                    ""status"": 0,
                    ""message"": ""Try again later"",
                    ""error"": """"
                }");
            JsonResponses.Add("AddCompanyBranchAddress1", @"	{
                                        ""status"": 1,
                                        ""message"": ""Successfully Added Bank Branch Address"",
                                        ""data"": [     
                                            {
						                        ""CompanyBranchId"": 3,
                                                ""FullAddress"": ""Full Return Address 1""
					                        }],
                                        ""error"": []
                                    }");
            JsonResponses.Add("AddCompanyBranchAddress2", @"{
                    ""status"": 0,
                    ""message"": ""Try again later"",
                    ""error"": """"
                }");
            JsonResponses.Add("ValidateLogin1", @"	{
                                        ""status"": 1,
                                        ""message"": ""User Logged in successfully!"",
                                        ""data"": [
                                            {
                                                ""UserId"": 2,
                                                ""Name"": ""Logan"",
                                                ""Email"": ""s@s.com"",
                                                ""Phone"": ""0987654321"",
                                                ""FirmId"": 3,
                                                ""FirmName"": ""securitas"",
                                                ""PasswordTypeId"": 3
                                            }
                                        ],
                                        ""error"": [],
                                        ""token"": ""eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyIjoiMTExMTExMTEiLCJpYXQiOjE1NDIyMTI1MDgsImV4cCI6MTU0MjIxNTUwOH0.kZXt1uN4dvYygvsADkvxzkwN1J19Cu-wT0XtqzwzMRc""
                                    }");
            JsonResponses.Add("ValidateLogin2", @"{
                              ""status"": 1,
                              ""message"": ""User Logged in successfully"",
                              ""data"": [{ 
	                                  ""passwordTypeId"": ""1"",
	                                  ""duration"": ""time""

                                     }],
                              ""JWT"": ""token"",
                              ""error"": null
                            }");
            JsonResponses.Add("ValidateLogin3", @"{
                              ""status"": 1,
                              ""message"": ""User Logged in successfully"",
                              ""data"": [{ 
	                                  ""passwordTypeId"": ""2"",
	                                  ""duration"": ""time""

                                     }],
                              ""JWT"": ""token"",
                              ""error"": null
                            }");
            JsonResponses.Add("ValidateLogin4", @"{
                            ""status"": 0,
                            ""message"": ""Invalid User Name or Password"",
                            ""data"": [{
		
                                        }],
                            ""error"": """"
                            }");
            JsonResponses.Add("ValidateEmail1", @"{
                            ""status"": 1,
                            ""message"": ""A temporary password has sent to your email id"",
                            ""data"": [{ 

  	                            }],
                            ""error"": null
                        }");
            JsonResponses.Add("ValidateEmail2", @"{
                            ""status"": 0,
                            ""message"": ""Invalid Email"",
                            ""data"": [{

                                        }],
                            ""error"": """"
                        }");

            JsonResponses.Add("UserRegistration1", @"{
                            ""status"": 1,
                            ""message"": ""Registered successfully"",
                            ""data"": [{ 
                                ""Name"": ""firm name""
  	                            }],
                            ""error"": null
                        }");
            JsonResponses.Add("UserRegistration2", @"{
                            ""status"": 0,
                            ""message"": ""Try again later"",
                            ""data"": [{

                                        }],
                            ""error"": """"
                        }");

            JsonResponses.Add("UserProfile1", @"{
                          ""status"": 1,
                          ""message"": ""Successfully fetched user profile"",
                          ""data"": [{ 
                              ""firmId"": 1,
                              ""firmName"": ""firm name"",
                              ""userId"": 1,
                              ""name"": ""name"",
                              ""email"": ""email"",
                              ""phone"": ""phone""

                                 }],
                          ""error"": null
                        }");
            JsonResponses.Add("UserProfile2", @"{
                          ""status"": 0,
                          ""message"": ""Try again later"",
                          ""data"": [{

                                      }],
                          ""error"": """"
                        }");
            JsonResponses.Add("RMAType1", @"{
                          ""status"": 1,
                          ""message"": ""Successfully fetched RMA type"",
                          ""data"": [
        {
            ""RMATypeId"": 1,
            ""Description"": ""Credit Request""
        },
        {
            ""RMATypeId"": 2,
            ""Description"": ""Service Request""
        }
    ], 
                          ""error"": null
                        }");
            JsonResponses.Add("RMAType2", @"{
                          ""status"": 0,
                          ""message"": ""Try again later"",
                          ""data"": [{

                                      }],
                          ""error"": """"
                        }");
            JsonResponses.Add("CreditReason1", @"{
                      ""status"": 1,
                      ""message"": ""Successfully fetched credit reason"",
                     ""data"": [
        {
            ""CreditReasonId"": 1,
            ""Description"": ""None""
        },
        {
            ""CreditReasonId"": 2,
            ""Description"": ""Job Cancelled""
        },
        {
            ""CreditReasonId"": 3,
            ""Description"": ""No Longer Required""
        },
        {
            ""CreditReasonId"": 4,
            ""Description"": ""Wrong Part Order""
        },
        {
            ""CreditReasonId"": 5,
            ""Description"": ""Wrong Color""
        }
    ], 
                      ""error"": null
                    }");
            JsonResponses.Add("CreditReason2", @"{
                      ""status"": 0,
                      ""message"": ""Try again later"",
                      ""data"": [{

                                  }],
                      ""error"": """"
                    }");

            JsonResponses.Add("ReturnAddress1", @"{
                      ""status"": 1,
                      ""message"": ""Successfully fetched return address"",
                        ""data"": [
        {
            ""FullAddress"": ""Full Return Address 1"", ""ReturnAddressId"": 1,
            ""Street"": ""16 secure lane"",
            ""City"": ""Security"",
            ""StateName"": ""Colorado"",
            ""IsDefault"": false
        },
        {
            ""FullAddress"": ""Full Return Address 2"", ""ReturnAddressId"": 2,
            ""Street"": ""500 Bi County Blvd"",
            ""City"": ""Famingdale"",
            ""StateName"": ""New York"",
            ""IsDefault"": true
        },
        {
            ""FullAddress"": ""Full Return Address 3"", ""ReturnAddressId"": 3,
            ""Street"": ""2495"",
            ""City"": ""New York"",
            ""StateName"": ""New York"",
            ""IsDefault"": true
        },
        {
            ""FullAddress"": ""Full Return Address 4"", ""ReturnAddressId"": 4,
            ""Street"": ""65 vortex"",
            ""City"": ""Fort Lauderdale"",
            ""StateName"": ""Florida"",
            ""IsDefault"": false
        },
        {
            ""FullAddress"": ""Full Return Address 5"", ""ReturnAddressId"": 5,
            ""Street"": ""35 Debevoise"",
            ""City"": ""Roosevelt"",
            ""StateName"": ""New York"",
            ""IsDefault"": true
        },
        {
            ""FullAddress"": ""Full Return Address 6"", ""ReturnAddressId"": 6,
            ""Street"": ""7008 Cursus Road"",
            ""City"": ""Quilleco"",
            ""StateName"": ""New York"",
            ""IsDefault"": false
        },
        {
            ""FullAddress"": ""Full Return Address 7"", ""ReturnAddressId"": 7,
            ""Street"": ""293-6240 Mauris. St."",
            ""City"": ""San Antonio"",
            ""StateName"": ""New York"",
            ""IsDefault"": false
        },
        {
            ""FullAddress"": ""Full Return Address 8"", ""ReturnAddressId"": 8,
            ""Street"": ""3084 Vel St."",
            ""City"": ""Augusta"",
            ""StateName"": ""New York"",
            ""IsDefault"": false
        },
        {
            ""FullAddress"": ""Full Return Address 9"", ""ReturnAddressId"": 9,
            ""Street"": ""397 Donec Av."",
            ""City"": ""Lauterach"",
            ""StateName"": ""New York"",
            ""IsDefault"": false
        },
        {
            ""FullAddress"": ""Full Return Address 10"", ""ReturnAddressId"": 10,
            ""Street"": ""P.O. Box 602, 4191 Fusce Road"",
            ""City"": ""Oetingen"",
            ""StateName"": ""New York"",
            ""IsDefault"": false
        },
        {
            ""FullAddress"": ""Full Return Address 11"", ""ReturnAddressId"": 11,
            ""Street"": ""668-8946 Diam Road"",
            ""City"": ""Lincoln"",
            ""StateName"": ""New York"",
            ""IsDefault"": false
        },
        {
            ""FullAddress"": ""Full Return Address 12"", ""ReturnAddressId"": 12,
            ""Street"": ""Ap #304-5517 Metus St."",
            ""City"": ""Dokkum"",
            ""StateName"": ""New York"",
            ""IsDefault"": false
        },
        {
            ""FullAddress"": ""Full Return Address 13"", ""ReturnAddressId"": 13,
            ""Street"": ""Ap #366-6717 Nostra, Rd."",
            ""City"": ""Denver"",
            ""StateName"": ""New York"",
            ""IsDefault"": false
        },
        {
            ""FullAddress"": ""Full Return Address 14"", ""ReturnAddressId"": 14,
            ""Street"": ""Ap #396-7077 Dui. St."",
            ""City"": ""Moorsele"",
            ""StateName"": ""New York"",
            ""IsDefault"": false
        },
        {
            ""FullAddress"": ""Full Return Address 15"", ""ReturnAddressId"": 15,
            ""Street"": ""156-8510 Sapien, Street"",
            ""City"": ""Squillace"",
            ""StateName"": ""New York"",
            ""IsDefault"": false
        }
    ],
                      ""error"": null
                    }");

            JsonResponses.Add("ReturnAddress2", @"{
                      ""status"": 0,
                      ""message"": ""Try again later"",
                      ""data"": [{

                                  }],
                      ""error"": """"
                    }");

            JsonResponses.Add("State1", @"{
                      ""status"": 1,
                      ""message"": ""Successfully fetched state"",
                      ""data"": [
                                    {
                                        ""StateId"": 1,
                                        ""Name"": ""Alabama"",
                                        ""Abreviation"": ""AL""
                                    },
                                    {
                                        ""StateId"": 2,
                                        ""Name"": ""Alaska"",
                                        ""Abreviation"": ""AK""
                                    },
                                    {
                                        ""StateId"": 3,
                                        ""Name"": ""Arizona"",
                                        ""Abreviation"": ""AZ""
                                    },
                                    {
                                        ""StateId"": 4,
                                        ""Name"": ""Arkansas"",
                                        ""Abreviation"": ""AR""
                                    },
                                    {
                                        ""StateId"": 5,
                                        ""Name"": ""California"",
                                        ""Abreviation"": ""CA""
                                    },
                                    {
                                        ""StateId"": 6,
                                        ""Name"": ""Colorado"",
                                        ""Abreviation"": ""CO""
                                    },
                                    {
                                        ""StateId"": 7,
                                        ""Name"": ""Connecticut"",
                                        ""Abreviation"": ""CT""
                                    },
                                    {
                                        ""StateId"": 8,
                                        ""Name"": ""Delaware"",
                                        ""Abreviation"": ""DE""
                                    },
                                    {
                                        ""StateId"": 9,
                                        ""Name"": ""District of Columbia"",
                                        ""Abreviation"": ""DC""
                                    },
                                    {
                                        ""StateId"": 10,
                                        ""Name"": ""Florida"",
                                        ""Abreviation"": ""FL""
                                    },
                                    {
                                        ""StateId"": 11,
                                        ""Name"": ""Georgia"",
                                        ""Abreviation"": ""GA""
                                    },
                                    {
                                        ""StateId"": 12,
                                        ""Name"": ""Hawaii"",
                                        ""Abreviation"": ""HI""
                                    },
                                    {
                                        ""StateId"": 13,
                                        ""Name"": ""Idaho"",
                                        ""Abreviation"": ""ID""
                                    },
                                    {
                                        ""StateId"": 14,
                                        ""Name"": ""Illinois"",
                                        ""Abreviation"": ""IL""
                                    },
                                    {
                                        ""StateId"": 15,
                                        ""Name"": ""Indiana"",
                                        ""Abreviation"": ""IN""
                                    },
                                    {
                                        ""StateId"": 16,
                                        ""Name"": ""Iowa"",
                                        ""Abreviation"": ""IA""
                                    },
                                    {
                                        ""StateId"": 17,
                                        ""Name"": ""Kansas"",
                                        ""Abreviation"": ""KS""
                                    },
                                    {
                                        ""StateId"": 18,
                                        ""Name"": ""Kentucky"",
                                        ""Abreviation"": ""KY""
                                    },
                                    {
                                        ""StateId"": 19,
                                        ""Name"": ""Louisiana"",
                                        ""Abreviation"": ""LA""
                                    },
                                    {
                                        ""StateId"": 20,
                                        ""Name"": ""Maine"",
                                        ""Abreviation"": ""ME""
                                    },
                                    {
                                        ""StateId"": 21,
                                        ""Name"": ""Maryland"",
                                        ""Abreviation"": ""MD""
                                    },
                                    {
                                        ""StateId"": 22,
                                        ""Name"": ""Massachusetts"",
                                        ""Abreviation"": ""MA""
                                    },
                                    {
                                        ""StateId"": 23,
                                        ""Name"": ""Michigan"",
                                        ""Abreviation"": ""MI""
                                    },
                                    {
                                        ""StateId"": 24,
                                        ""Name"": ""Minnesota"",
                                        ""Abreviation"": ""MN""
                                    },
                                    {
                                        ""StateId"": 25,
                                        ""Name"": ""Mississippi"",
                                        ""Abreviation"": ""MS""
                                    },
                                    {
                                        ""StateId"": 26,
                                        ""Name"": ""Missouri"",
                                        ""Abreviation"": ""MO""
                                    },
                                    {
                                        ""StateId"": 27,
                                        ""Name"": ""Montana"",
                                        ""Abreviation"": ""MT""
                                    },
                                    {
                                        ""StateId"": 28,
                                        ""Name"": ""Nebraska"",
                                        ""Abreviation"": ""NE""
                                    },
                                    {
                                        ""StateId"": 29,
                                        ""Name"": ""Nevada"",
                                        ""Abreviation"": ""NV""
                                    },
                                    {
                                        ""StateId"": 30,
                                        ""Name"": ""New Hampshire"",
                                        ""Abreviation"": ""NH""
                                    },
                                    {
                                        ""StateId"": 31,
                                        ""Name"": ""New Jersey"",
                                        ""Abreviation"": ""NJ""
                                    },
                                    {
                                        ""StateId"": 32,
                                        ""Name"": ""New Mexico"",
                                        ""Abreviation"": ""NM""
                                    },
                                    {
                                        ""StateId"": 33,
                                        ""Name"": ""New York"",
                                        ""Abreviation"": ""NY""
                                    },
                                    {
                                        ""StateId"": 34,
                                        ""Name"": ""North Carolina"",
                                        ""Abreviation"": ""NC""
                                    },
                                    {
                                        ""StateId"": 35,
                                        ""Name"": ""North Dakota"",
                                        ""Abreviation"": ""ND""
                                    },
                                    {
                                        ""StateId"": 36,
                                        ""Name"": ""Ohio"",
                                        ""Abreviation"": ""OH""
                                    },
                                    {
                                        ""StateId"": 37,
                                        ""Name"": ""Oklahoma"",
                                        ""Abreviation"": ""OK""
                                    },
                                    {
                                        ""StateId"": 38,
                                        ""Name"": ""Oregon"",
                                        ""Abreviation"": ""OR""
                                    },
                                    {
                                        ""StateId"": 39,
                                        ""Name"": ""Pennsylvania"",
                                        ""Abreviation"": ""PA""
                                    },
                                    {
                                        ""StateId"": 40,
                                        ""Name"": ""Puerto Rico"",
                                        ""Abreviation"": ""PR""
                                    },
                                    {
                                        ""StateId"": 41,
                                        ""Name"": ""Rhode Island"",
                                        ""Abreviation"": ""RI""
                                    },
                                    {
                                        ""StateId"": 42,
                                        ""Name"": ""South Carolina"",
                                        ""Abreviation"": ""SC""
                                    },
                                    {
                                        ""StateId"": 43,
                                        ""Name"": ""South Dakota"",
                                        ""Abreviation"": ""SD""
                                    },
                                    {
                                        ""StateId"": 44,
                                        ""Name"": ""Tennessee"",
                                        ""Abreviation"": ""TN""
                                    },
                                    {
                                        ""StateId"": 45,
                                        ""Name"": ""Texas"",
                                        ""Abreviation"": ""TX""
                                    },
                                    {
                                        ""StateId"": 46,
                                        ""Name"": ""Utah"",
                                        ""Abreviation"": ""UT""
                                    },
                                    {
                                        ""StateId"": 47,
                                        ""Name"": ""Vermont"",
                                        ""Abreviation"": ""VT""
                                    },
                                    {
                                        ""StateId"": 48,
                                        ""Name"": ""Virginia"",
                                        ""Abreviation"": ""VA""
                                    },
                                    {
                                        ""StateId"": 49,
                                        ""Name"": ""Washington"",
                                        ""Abreviation"": ""WA""
                                    },
                                    {
                                        ""StateId"": 50,
                                        ""Name"": ""West Virginia"",
                                        ""Abreviation"": ""WV""
                                    },
                                    {
                                        ""StateId"": 51,
                                        ""Name"": ""Wisconsin"",
                                        ""Abreviation"": ""WI""
                                    },
                                    {
                                        ""StateId"": 52,
                                        ""Name"": ""Wyoming"",
                                        ""Abreviation"": ""WY""
                                    }
                                ],
                      ""error"": null
                    }");

            JsonResponses.Add("State2", @"{
                      ""status"": 0,
                      ""message"": ""Try again later"",
                      ""data"": [{

                                  }],
                      ""error"": """"
                    }");

            
            JsonResponses.Add("BankName1", @"{
                  ""status"": 1,
                  ""message"": ""Successfully fetched bank name"",
                  ""data"": [
                         {    
                            ""CompanyId"": 1,
                            ""Name"": ""Beth FCU"",
                            ""LongName"": ""Bethpage FCU""
                        },
                        {
                            ""CompanyId"": 2,
                            ""Name"": ""BNB"",
                            ""LongName"": ""Bridgehampton National Bank""
                        },
                        {
                            ""CompanyId"": 3,
                            ""Name"": ""BOA"",
                            ""LongName"": ""Bank Of America""
                        },
                        {
                            ""CompanyId"": 4,
                            ""Name"": ""CO"",
                            ""LongName"": ""Capital One""
                        },
                        {
                            ""CompanyId"": 5,
                            ""Name"": ""HSBC"",
                            ""LongName"": ""Hongkong and Shanghai Banking Corporation""
                        },
                        {
                            ""CompanyId"": 6,
                            ""Name"": ""RB"",
                            ""LongName"": ""Regions Bank""
                        }
  	                   ],
                  ""error"": null
                }");

            JsonResponses.Add("BankName2", @"{
                  ""status"": 0,
                  ""message"": ""Try again later"",
                  ""data"": [{

                              }],
                  ""error"": """"
                }");

            JsonResponses.Add("CompanyAddress1", @"{
                  ""status"": 1,
                  ""message"": ""Successfully fetched bank address"",
                  ""data"": [
                            {
                                ""CompanyBranchId"": 1,
                                ""FullAddress"": ""Full Address 1""
                            },
                            {
                                ""CompanyBranchId"": 2,
                                ""FullAddress"": ""Full Address 2""
                            }
                            ],
                  ""error"": null
                }");

            JsonResponses.Add("CompanyAddress2", @"{
                  ""status"": 0,
                  ""message"": ""Try again later"",
                  ""data"": [{

                              }],
                  ""error"": """"
                }");

            JsonResponses.Add("DispatchReason1", @"{
                  ""status"": 1,
                  ""message"": ""Successfully fetched dispatch reason"",
                  ""data"": [
        {
            ""DispatchReasonId"": 1,
            ""DispatchReasonName"": null,
            ""DispatchReasonDescription"": ""Reader flashing red/access denied""
        },
        {
            ""DispatchReasonId"": 2,
            ""DispatchReasonName"": null,
            ""DispatchReasonDescription"": ""Door not opening""
        },
        {
            ""DispatchReasonId"": 3,
            ""DispatchReasonName"": null,
            ""DispatchReasonDescription"": ""Phones/Contactless cards not being read""
        }
    ], 
                  ""error"": null
                }");

            JsonResponses.Add("DispatchReason2", @"{
                  ""status"": 0,
                  ""message"": ""Try again later"",
                  ""data"": [{

                              }],
                  ""error"": """"
                }");

            JsonResponses.Add("Faults1", @"{
                  ""status"": 1,
                  ""message"": ""Successfully fetched problem description"",
                  
""data"": [
        {
            ""FaultId"": 1,
            ""FaultDescription"": ""Cable cut alarm active""
        },
        {
            ""FaultId"": 2,
            ""FaultDescription"": ""Card Swipe not reading cards""
        },
        {
            ""FaultId"": 3,
            ""FaultDescription"": ""False skimming/overlay alarm""
        },
        {
            ""FaultId"": 4,
            ""FaultDescription"": ""MMR does not read""
        },
        {
            ""FaultId"": 5,
            ""FaultDescription"": ""NONE GIVEN""
        },
        {
            ""FaultId"": 6,
            ""FaultDescription"": ""No LED""
        },
        {
            ""FaultId"": 7,
            ""FaultDescription"": ""Other""
        },
        {
            ""FaultId"": 8,
            ""FaultDescription"": ""Reader LED not working""
        },
        {
            ""FaultId"": 9,
            ""FaultDescription"": ""Trouble loading firmware""
        },
        {
            ""FaultId"": 10,
            ""FaultDescription"": ""NFG""
        },
        {
            ""FaultId"": 11,
            ""FaultDescription"": ""Tamper alarm active""
        },
        {
            ""FaultId"": 12,
            ""FaultDescription"": ""Can't connect to controller""
        },
        {
            ""FaultId"": 13,
            ""FaultDescription"": ""Controller unresponsive""
        },
        {
            ""FaultId"": 14,
            ""FaultDescription"": ""Wrong Part Ordered""
        },
        {
            ""FaultId"": 15,
            ""FaultDescription"": ""Reader flashing red/access denied""
        },
        {
            ""FaultId"": 16,
            ""FaultDescription"": ""Door not opening""
        },
        {
            ""FaultId"": 17,
            ""FaultDescription"": ""Phones/Contactless cards not being read""
        }
    ], 
                  ""error"": null
                }");

            JsonResponses.Add("Faults2", @"{
                  ""status"": 0,
                  ""message"": ""Try again later"",
                  ""data"": [{

                              }],
                  ""error"": """"
                }");

            JsonResponses.Add("Part1", @"{
                  ""status"": 1,
                  ""message"": ""Successfully fetched part"",
                  ""data"": [
            {
                ""PartId"": 1,
                ""PartDescription"": ""ACS-1Ev2"",
                ""PartNumber"": ""200-10095"",
                ""SerialNumber"": ""10894 "",
            } ],  
                  ""error"": null
                }");

            JsonResponses.Add("Part2", @"{
                  ""status"": 0,
                  ""message"": ""Try again later"",
                  ""data"": [{

                              }],
                  ""error"": """"
                }");

            JsonResponses.Add("SubmitRMARequest1", @"{
                    ""status"": 1,
                    ""message"": ""RMA Request submitted successfully"",
                    ""data"": [{ 

  	                    }],
                    ""error"": null
                }");

            JsonResponses.Add("SubmitRMARequest2", @"{
                    ""status"": 0,
                    ""message"": ""Try again later"",
                    ""data"": [{

                                }],
                    ""error"": """"
                }");

    }
        static TestAPI()
        {
            InitJsons();
        }
        public static Tuple<HttpResponseMessage, string> TestPost(string name, int type)
        {
            string ResponseString = JsonResponses[name+type.ToString()];
            HttpResponseMessage response_msg = new HttpResponseMessage(HttpStatusCode.OK);
            response_msg.Content = new StringContent(ResponseString, Encoding.UTF8, "application/json");
            return Tuple.Create(response_msg, ResponseString);
        }
        
    }
}
