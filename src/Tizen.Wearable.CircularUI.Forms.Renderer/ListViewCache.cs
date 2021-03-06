﻿/*
 * Copyright (c) 2018 Samsung Electronics Co., Ltd All Rights Reserved
 *
 * Licensed under the Flora License, Version 1.1 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://floralicense.org/license/
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using ElmSharp;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;
using Xamarin.Forms.Internals;

namespace Tizen.Wearable.CircularUI.Forms.Renderer
{
    public static class ListViewCache
    {
        static readonly Dictionary<Type, CellRenderer> _cellRendererCache = new Dictionary<Type, CellRenderer>();
        static GenItemClass _informalItemClass;
        static GenItemClass _paddingItemClass;
        static GroupTextCellRenderer _groupCache;

        public static CellRenderer Get(Cell cell, bool IsGroupHeader = false)
        {
            if (IsGroupHeader)
            {
                if (_groupCache == null)
                {
                    _groupCache = new GroupTextCellRenderer();
                }
                return _groupCache;
            }

            Type type = cell.GetType();
            CellRenderer renderer;
            if (_cellRendererCache.TryGetValue(type, out renderer))
            {
                return renderer;
            }
            else
            {
                renderer = Registrar.Registered.GetHandler<CellRenderer>(type);
                if (renderer == null)
                {
                    throw new ArgumentNullException("Unsupported cell type");
                }
                return _cellRendererCache[type] = renderer;
            }
        }

        public static GenItemClass InformalItemClass => _informalItemClass ?? (_informalItemClass = new HeaderOrFooterItemClass());
        public static GenItemClass PaddingItemClass => _paddingItemClass ?? (_paddingItemClass = new PaddingItemClass());
    }

    public class HeaderOrFooterItemClass : GenItemClass
    {
        static int ToPx(double dp)
        {
            return (int)Math.Round(dp * Device.Info.ScalingFactor);
        }
        public HeaderOrFooterItemClass() : base("full")
        {
            GetContentHandler = OnGetContent;
        }

        protected EvasObject OnGetContent(object data, string part)
        {
            var ctx = data as TypedItemContext;
            if (ctx != null)
            {
                var element = ctx.Element;
                var renderer = Platform.GetOrCreateRenderer(element);
                if (element.MinimumHeightRequest == -1)
                {
                    var request = element.Measure(double.PositiveInfinity, double.PositiveInfinity);
                    renderer.NativeView.MinimumHeight = ToPx(request.Request.Height);
                }
                else
                {
                    renderer.NativeView.MinimumHeight = ToPx(element.MinimumHeightRequest);
                }
                //Console.WriteLine($"{ctx.Type.ToString()} / renderer.NativeView.MinimumHeight = {renderer.NativeView.MinimumHeight}");
                (renderer as LayoutRenderer)?.RegisterOnLayoutUpdated();
                return renderer.NativeView;
            }
            return null;
        }
    }

    public class PaddingItemClass : GenItemClass
    {
        public PaddingItemClass() : base("padding")
        {
        }
    }
}
