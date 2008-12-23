// Copyright 2007-2008 The Apache Software Foundation.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace Magnum.Common.Specs.StateMachine
{
	using System;
	using System.Runtime.Serialization;
	using Common.StateMachine;

	public class AccountManagementWorkflow :
		StateMachine<AccountManagementWorkflow>
	{
		static AccountManagementWorkflow()
		{
			Define(() =>
				{
					// eliminated, ceremony, just use the default states with the right names
					// SetInitialState(Initial);
					// SetCompletedState(Completed);

					During(Initial,
						When(RequestSubmitted, machine =>
							{
								// machine is your scope, maybe rename to workflow. 
								// you don't want statics since the instance carries your state
								// and any other data you capture
								machine.RequestApprovalFromManager();
								machine.TransitionTo(WaitingForManagementResponse);
							}),
						When(RequestCanceled, machine =>
							{
								// nothing has happened yet so we just complete 
								machine.Complete();
							}));

					During(WaitingForManagementResponse,
						When(ManagementApproves, machine =>
							{
								machine.RequestSecurityReview();
								machine.TransitionTo(WaitingForSecurityResponse);
							}),
						When(ManagementDeclines, machine =>
							{
								machine.NotifyRequestDeclined();
								machine.Complete();
							}),
						When(RequestCanceled, machine =>
							{
								machine.NotifyRequestCancelled();
								machine.Complete();
							}));

					During(WaitingForSecurityResponse,
						When(SecurityApproves, machine =>
							{
								machine.NotifyWorkTeams(); //possible fork/join here
								machine.TransitionTo(WaitingForWorkToComplete);
							}),
						When(SecurityDeclines, machine =>
							{
								machine.NotifyRequestDeclined();
								machine.Complete();
							}));

					During(WaitingForWorkToComplete,
						When(WorkComplete, machine =>
							{
								// do stuff
							}));
					During(Completed,
						When(Completed.Enter, machine =>
							{
								// complete the transaction if required
							}));
				});
		}

		public AccountManagementWorkflow()
		{
		}

		public AccountManagementWorkflow(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public static State Initial { get; set; }
		public static State WaitingForManagementResponse { get; set; }
		public static State WaitingForSecurityResponse { get; set; }
		public static State WaitingForWorkToComplete { get; set; }
		public static State Completed { get; set; }

		public static Event RequestSubmitted { get; set; }
		public static Event ManagementApproves { get; set; }
		public static Event ManagementDeclines { get; set; }
		public static Event SecurityApproves { get; set; }
		public static Event SecurityDeclines { get; set; }
		public static Event WorkComplete { get; set; }
		public static Event RequestCanceled { get; set; }

		public void SubmitOrder()
		{
			RaiseEvent(RequestSubmitted);
		}

		public void SubmitPayment()
		{
			RaiseEvent(ManagementApproves);
		}

		//what are these called
		private void NotifyRequestCancelled()
		{
			throw new NotImplementedException();
		}

		private void NotifyWorkTeams()
		{
			throw new NotImplementedException();
		}

		private void NotifyRequestDeclined()
		{
			throw new NotImplementedException();
		}

		private void RequestSecurityReview()
		{
			throw new NotImplementedException();
		}

		private void RequestApprovalFromManager()
		{
			throw new NotImplementedException();
		}
	}
}