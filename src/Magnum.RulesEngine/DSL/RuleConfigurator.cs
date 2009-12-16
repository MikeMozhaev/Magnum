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
namespace Magnum.RulesEngine.DSL
{
	public interface RuleConfigurator<TRule>
		where TRule : class
	{
		ConditionConfigurator<TRule, TCondition> When<TCondition>()
			where TCondition : class;

		RuleConfigurator<TRule> Then<TConsequence>()
			where TConsequence : Consequence<TRule>;

		RuleConfigurator<TRule> Always<TConsequence>()
			where TConsequence : Consequence<TRule>;
	}
}