namespace RTS.Engine.Input;

abstract record class InputEvent();

record class Quit() : InputEvent;
