using Godot;
using System;

public interface IIntractable
{
    void Pickup(ControllerService controller);
    void Drop(ControllerService controller);

}
