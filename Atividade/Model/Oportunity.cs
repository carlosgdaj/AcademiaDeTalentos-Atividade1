using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atividade.Model
{
    class Oportunity
    {
        public IOrganizationService Service { get; set; }

        public string TableName = "opportunity";

        public Oportunity(IOrganizationService service)
        {
            this.Service = service;
        }
        public EntityCollection RetrieveMultipleAccountByOportunity(Guid oportunityId)
        {
            QueryExpression queryOportunity = new QueryExpression(this.TableName);
            queryOportunity.ColumnSet.AddColumns("name", "parentaccountid", "totallineitemamount");
            queryOportunity.Criteria.AddCondition("opportunityid", ConditionOperator.Equal, oportunityId);

            return this.Service.RetrieveMultiple(queryOportunity);
        }
        public void UpdateOpportunity(Guid idOpotunity, double desconto)
        {
            Entity opportunity = new Entity(this.TableName);
            opportunity.Id = idOpotunity;
            opportunity["discountamount"] = desconto;
            this.Service.Update(opportunity);
        }
    }
}
