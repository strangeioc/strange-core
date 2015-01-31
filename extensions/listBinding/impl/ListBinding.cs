/*
 * Copyright 2013 ThirdMotion, Inc.
 *
 *	Licensed under the Apache License, Version 2.0 (the "License");
 *	you may not use this file except in compliance with the License.
 *	You may obtain a copy of the License at
 *
 *		http://www.apache.org/licenses/LICENSE-2.0
 *
 *		Unless required by applicable law or agreed to in writing, software
 *		distributed under the License is distributed on an "AS IS" BASIS,
 *		WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *		See the License for the specific language governing permissions and
 *		limitations under the License.
 */

using strange.extensions.injector.api;
using strange.extensions.listBind.api;
using strange.framework.api;
using strange.framework.impl;
using System;
using System.Collections.Generic;

namespace strange.extensions.listBind.impl
{
    public class ListBinding : Binding, IListBinding
    {
        private readonly List<IInjectionBinding> bindings = new List<IInjectionBinding>();

        private readonly IInjectionBinder injectionBinder;

        public ListBinding(Binder.BindingResolver resolver, IInjectionBinder injectionBinder)
            : base(resolver)
		{
            this.injectionBinder = injectionBinder;
		}

        public Type ListType { get; set; }

        public List<IInjectionBinding> Bindings { get { return this.bindings;  } }

        public new IInjectionBinding To<T>()
        {
            var binding = injectionBinder.Bind(ListType).To<T>().ToName(typeof(T).ToString());
            bindings.Add(binding);
            //var binding = base.To<T>().ToName(typeof(T).ToString());
            //this.bindings.Add(binding);
            return binding;

        }

        public IInjectionBinding ToValue(object o)
        {
            var binding = injectionBinder.Bind(ListType).ToValue(o).ToName(o.ToString());
            bindings.Add(binding);
            return binding;
        }

    }
}
