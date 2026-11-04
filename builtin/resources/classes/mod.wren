class Mod {
  systems { _systems }
  components { _components }

  construct new(modSystems, modComponents) {
    _systems = modSystems
    _components = modComponents
  }
}
