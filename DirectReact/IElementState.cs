﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact
{
    public interface IElementState<Renderer> : IDisposable
        where Renderer : IRenderer<Renderer>
    {
        void Render(Renderer r);
        Bounds BoundingBox { get; }

        void OnMouseClick(ClickEvent click);
    }
}
