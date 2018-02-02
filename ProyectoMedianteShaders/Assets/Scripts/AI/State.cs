abstract public class State {

    abstract public void OnEnter(Agent a);

    abstract public void Update(Agent a, float dt);

    abstract public void OnExit(Agent a);

}
