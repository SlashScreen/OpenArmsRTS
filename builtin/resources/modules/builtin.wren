foreign class Engine {
    foreign static testMe()
    foreign static addSystem(system)
}

foreign class System() {
  construct new() { }

  foreign static query(types) // TODO: figure out arity and return type
}
