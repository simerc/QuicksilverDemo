using Mediachase.Commerce.WorkflowCompatibility;

namespace Mediachase.Commerce.Workflow.Activities.ReturnForm
{
    public class CalculateReturnFormTaxActivity : ReturnFormBaseActivity
    {
        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            try
            {

                // Calculate sale tax
                CalculateSaleTaxes();

                // Retun the closed status indicating that this activity is complete.
                return ActivityExecutionStatus.Closed;
            }
            catch
            {
                // An unhandled exception occured.  Throw it back to the WorkflowRuntime.
                throw;
            }
        }

        /// <summary>
        /// Calculates the sale taxes.
        /// </summary>
        private void CalculateSaleTaxes()
        {
            var order = base.ReturnOrderForm.Parent;
            OrderFormHelper.CalculateTaxes(base.ReturnOrderForm, order.MarketId, order.BillingCurrency);
        }
    }
}
