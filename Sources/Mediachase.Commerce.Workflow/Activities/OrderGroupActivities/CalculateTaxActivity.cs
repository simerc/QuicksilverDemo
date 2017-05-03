using EPiServer.Logging;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.WorkflowCompatibility;
using System;

namespace Mediachase.Commerce.Workflow.Activities
{
    public class CalculateTaxActivity : OrderGroupActivityBase
    {
        /// <summary>
        /// Called by the workflow runtime to execute an activity.
        /// </summary>
        /// <param name="executionContext">The <see cref="T:Mediachase.Commerce.WorkflowCompatibility.ActivityExecutionContext"/> to associate with this <see cref="T:Mediachase.Commerce.WorkflowCompatibility.Activity"/> and execution.</param>
        /// <returns>
        /// The <see cref="T:Mediachase.Commerce.WorkflowCompatibility.ActivityExecutionStatus"/> of the run task, which determines whether the activity remains in the executing state, or transitions to the closed state.
        /// </returns>
        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            try
            {
                // Validate the properties at runtime
                ValidateRuntime();

                // Calculate taxes
                CalculateTaxes();

                // Return the closed status indicating that this activity is complete.
                return ActivityExecutionStatus.Closed;
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(GetType()).Error(ex.Message, ex);
                // An un-handled exception occurred. Throw it back to the WorkflowRuntime.
                throw;
            }
        }

        /// <summary>
        /// Calculates the sale and shipping taxes.
        /// </summary>
        private void CalculateTaxes()
        {
            // Get the property, since it is expensive process, make sure to get it once
            OrderGroup order = OrderGroup;

            foreach (OrderForm form in order.OrderForms)
            {
                OrderFormHelper.CalculateTaxes(form, order.MarketId, order.BillingCurrency);                
            }
        }
    }
}
