using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class CircuitCalculator : MonoBehaviour
{
    private const int angleDeltaAllowence = 5;
    public Circuit _circuit;
    public CircuitItem _startItem;
    public CircuitItem _endItem;
    public GameObject _pipeGO;
    public GameObject _elbowGO;
    private List<GameObject> gameObjectsInstantiated;


    public List<GameObject> CompleteCircuit() {
        return CompleteCircuit(_circuit, _startItem, _endItem, _pipeGO, _elbowGO, 0.26f,150);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) {
            CompleteCircuit();
        }
    }

    public List<GameObject> CompleteCircuit(Circuit circuit,CircuitItem startItem, CircuitItem endItem,GameObject pipeGO, GameObject elbowGO, float elbowHeight, float width)
    {
        gameObjectsInstantiated = new List<GameObject>();
        Vector3 startPos = startItem.endNormal.position; // la posicion de partida, al final del primer item
        Vector3 endPos = endItem.initialNormal.position;//la posicion final a la que quiero llegar, al principio del ultimo item
        Vector3 endRot = endItem.initialNormal.right;
        Vector3 startRot = startItem.endNormal.right;
        List<GameObject> newCircuitItems = new List<GameObject>();
        List<GameObject> lastCircuitItems = new List<GameObject>();
       // float elbowHeight = elbowGO.GetComponentInChildren<Measurements>().lenght;

        float angle = Vector3.Angle(startRot, endRot); //agrego el codo final para que la normal inicial y la final sean iguales
        if (angle > 3)
        { //si esta a mas de 3 grados pongo un codo 
            if (angle > 170) {
                (endPos,endRot) = FixEndRotation(circuit, endPos, endRot, startRot, elbowHeight, lastCircuitItems, elbowGO, width ,true);//pongo el primer codo sin importar el angulo

            }
            (endPos, endRot) = FixEndRotation(circuit,endPos, endRot, startRot, elbowHeight, lastCircuitItems, elbowGO, width);
        }

        Vector3 direction = startItem.endNormal.transform.right;
        float distance = SignedDistanceBetweenPointsInOneDirection(startPos, startPos + direction, endPos, direction);
        FixDirectionIfNeeded(ref direction, ref distance);
        direction= startItem.endNormal.transform.right;
        Vector3 intermediatePosition = startPos + (direction * distance);
        print("distance " + Vector3.Distance(intermediatePosition, endPos));

        var pipe = AddPipesNedeed(startPos, endPos, pipeGO, elbowHeight, distance, intermediatePosition, 1.5f,width,circuit); //agrego los pipes necesarios en la derecha
        if (pipe != null)
        {
            newCircuitItems.Add(pipe);
            var pipeItem = pipe.GetComponent<CircuitItem>();
            startPos = pipeItem.endNormal.position;
        }
        direction = endPos - intermediatePosition;
        CalculatePipesAndElbowsInADirection(ref startPos, endPos, newCircuitItems, pipeGO, elbowGO, elbowHeight,ref intermediatePosition,circuit,width);
        //agrgeo el codo final si es necesario
        if (Vector3.Distance(endPos, startPos) > 0.05f) {
            direction = startItem.endNormal.transform.right;
            Vector3 intermediatePositionElbow = intermediatePosition + (elbowHeight * direction);
            GameObject elbowf = AddElbow(circuit,startPos, intermediatePositionElbow, intermediatePosition, elbowGO,width);
            newCircuitItems.Add(elbowf);
        }

        newCircuitItems.AddRange(lastCircuitItems);

        return newCircuitItems;

    }
   
    private (Vector3,Vector3) FixEndRotation(Circuit circuit, Vector3 endPos, Vector3 endRot, Vector3 startRot, float elbowHeight, List<GameObject> objects,GameObject elbowGO, float width, bool allowAnyAngle=false)
    {
        CircuitItem c = Circuit.InstantiateGO(elbowGO, endPos, Quaternion.identity, width, circuit);
        GameObject go = c.gameObject;
        gameObjectsInstantiated.Add(go);
        var axis = c.endNormal.right;

        Quaternion rotation = Quaternion.FromToRotation(axis, endRot);
        go.transform.rotation = rotation;//acomodo la rotacion para que se conecte con el objeto anterior. 
        var e = go.GetComponent<Elbow>();
        var deltaAngle= float.MaxValue;
        for (int i = 0; i < 72; i++)//Lo voy girando 5 grados hasta que encuentro el angulo para que coincida la normal de inicio con la normal del pipe
        {
            deltaAngle = Vector3.Angle(startRot, c.initialNormal.transform.right);
            if (Mathf.Abs(deltaAngle) < 5 || allowAnyAngle) //admito hasta 5 grados
            {
                break; // estoy en la orientacion correcta
            }
            else
            {
                e.RotateFinalGrades(5);
            }
        }

        if (Mathf.Abs(deltaAngle) > angleDeltaAllowence && !allowAnyAngle) {
            DeleteAllInstantiatedGOs();
            throw new Exception("No hay ningun angulo posible para conectar con la siguiente tuberia");
        } 

        Vector3 offset = go.transform.position - c.endNormal.position;
        go.transform.position += offset;
        objects.Insert(0,go);
        endPos = c.initialNormal.position;
        endRot = c.initialNormal.right;
        return (endPos,endRot);
    }

    private static void FixDirectionIfNeeded(ref Vector3 direction, ref float distance)
    {
        if (distance < 0)
        { //la direccion es para el otro lado 
            distance = Mathf.Abs(distance);//la hago positiva
            direction *= -1; //cambio la direccion
        }
    }







    /// <summary>
    /// Add pipes and Elbows needed In a direction. Modify startPos, and add newCircuitItems
    /// </summary>
    private void CalculatePipesAndElbowsInADirection(ref Vector3 startPos, Vector3 endPos, List<GameObject> newCircuitItems, GameObject pipeGO, GameObject elbowGO, float elbowHeight, ref Vector3 intermediatePosition,Circuit circuit, float width)
    {
        var direction = intermediatePosition- endPos;
        float distance = SignedDistanceBetweenPointsInOneDirection(intermediatePosition, intermediatePosition + direction, endPos, direction); // si una vez que agregue el codo voy a necesitar ir a esa direccion
        FixDirectionIfNeeded(ref direction, ref distance);
        if (Mathf.Abs(distance) > 0.02)
        { // si la distancia es mayor necesito agrego el codo apuntando a la direccion correcta y tuberias.
          //TODO FALTA AGREGAR EL CODO LAS DOS POSICIONES INICIALES CAMBIARIAN
            //Vector3 intermediatePositionElbow= intermediatePosition + (elbowHeight * direction); 
            GameObject elbow = AddElbow(circuit,startPos, endPos, intermediatePosition,elbowGO,width);
            var elbowItem = elbow.GetComponent<CircuitItem>();
            newCircuitItems.Add(elbow);


            startPos = elbowItem.endNormal.transform.position;//Actualizo la posicion inicial a la posicion final del codo
            distance -= (elbowHeight/2);//TODO PORQUE FUNCIONA ESTO???????????
            intermediatePosition = startPos + (direction * distance);
            GameObject pipe = AddPipesNedeed(startPos, endPos, pipeGO, elbowHeight, distance, intermediatePosition,1,width,circuit); //agrego los pipes necesarios
            if (pipe != null)
            {
                var pipeItem = pipe.GetComponent<CircuitItem>();
                newCircuitItems.Add(pipe);
                startPos = pipeItem.endNormal.transform.position;//Actualizo la posicion inicial a la posicion final del codo
                //startPos = pipeItem.endNormal.position;
            }
        }
    }

    /// <summary>
    /// Instanciate a Elbow in a specific position and rotation
    /// </summary>
    private GameObject AddElbow(Circuit circuit, Vector3 startPos, Vector3 endPos, Vector3 intermediatePos, GameObject elbowGO, float width)
    {
       CircuitItem c = Circuit.InstantiateGO(elbowGO, startPos, Quaternion.identity, width, circuit);
        GameObject go = c.gameObject;
        gameObjectsInstantiated.Add(go);
        var axis = c.initialNormal.right;
        Vector3 initialDirection =  intermediatePos- startPos;


        Quaternion rotation = Quaternion.FromToRotation(axis, initialDirection);
        go.transform.rotation = rotation;//acomodo la rotacion para que se conecte con el objeto anterior. 

        var e = go.GetComponent<Elbow>();
        Vector3 finalDirection = endPos - intermediatePos;
        float angle=float.MinValue;
        for (int i = 0; i < 72; i++)//Lo voy girando de a 5 grados hasta que encuentro el angulo para la parte final, hasta 4 veces.
        {
             angle = Vector3.Angle(finalDirection, c.endNormal.transform.right);
            if (Mathf.Abs(angle) < 5) //admito hasta 5 grados
            {
                break; // estoy en la orientacion correcta
            }
            else {
                e.RotateGrades(5);
            }
        }

        if (Mathf.Abs(angle) > angleDeltaAllowence) {
            DeleteAllInstantiatedGOs();
            throw new Exception("No hay ningun angulo posible para conectar con la siguiente tuberia");
        } 
        MoveItem(go);
        return go;
    }


    /// <summary>
    /// Check if it is necesary to add a pipe. if it is needed, it instanciate it in the position and the rotation needed
    /// </summary>
    /// <return>   return the go instanciated or null </return>
    private GameObject AddPipesNedeed( Vector3 startPos, Vector3 endPos,GameObject pipeGO,  float elbowHeight, float distance, Vector3 intermediatePos, float deltaElbowHeight,float width, Circuit circuit)
    {
        GameObject go=null;
        if (Vector3.Distance(intermediatePos, endPos) < 0.02f)
        { // mido la distancia a la posicion final. Si es menor que 2 cm ya llegue.
            
            go= AddPipe( startPos, intermediatePos, distance, pipeGO, width,circuit); //agrego un pipe con la distancia necesaria para llegar.
            return go;
        }
        else
        { // si no es menor significa que voy a tener que agregar un codo despues, asi que le dejo espacio
            float pipeDistance = distance - (elbowHeight* deltaElbowHeight);//TODO VER PORQUE EN EL TUBO ES MAS LARGO, TENGO QUE RESTARLE DE VUELTA EL ELBOW? NO DEBERIA SER NECESARIO
            print("pipeDistance" + pipeDistance);
            if (pipeDistance < 0) {
                DeleteAllInstantiatedGOs();
                throw new Exception("there is not enought space");// no hay espacio ni para poner un codo
            } 
            else if (pipeDistance > 0.02f) go=AddPipe( startPos, intermediatePos, pipeDistance, pipeGO, width, circuit); // si hay espacio pongo un tubo con el largo necesario para despues poner un codo
            return go;
        }
    }

    /// <summary>
    /// Instanciate a pipe in a specific position and rotation
    /// </summary>
    /// <return>   return the go instanciated </return>
    private float SignedDistanceBetweenPointsInOneDirection(Vector3 lineP0, Vector3 lineP1, Vector3 planeP0, Vector3 planeN)
    {
        Vector3 intersecPos = MathUtilities.IntersecLineWithPlane(lineP0, lineP1, planeP0, planeN);
        var distance= Vector3.Distance(lineP0, intersecPos);
        var distance2= Vector3.Distance(lineP0+ (planeN*distance), intersecPos); // Si es 0 estoy en la direccion correcta si no es la otra direccion

        if (distance2 > distance) distance *= -1;
        return distance;
    }


    /// <summary>
    /// Instanciate a pipe in a specific position and rotation
    /// </summary>
    /// <return>   return the go instanciated </return>
    private GameObject AddPipe( Vector3 startPos, Vector3 finalPos, float distance, GameObject pipeGO, float width, Circuit circuit)
    {
        print("startPos " + startPos + " rotation " + finalPos + " distance " + distance.ToString());
        CircuitItem c = Circuit.InstantiateGO(pipeGO, startPos, Quaternion.identity, width, circuit);
        GameObject go = c.gameObject;
        gameObjectsInstantiated.Add(go);
        go.GetComponent<Pipes>().ScaleItemDiameter(width, width);
        float lenght = go.GetComponentInChildren<Measurements>().lenght;

        if (lenght == 0)
        {
            DeleteAllInstantiatedGOs();
            throw new Exception("You have to set a lenght for the pipe");
        }


        RotateItem(startPos, finalPos, go);
        ScaleItem(go, lenght, distance);
        MoveItem(go);
        return go;
    }

    private void DeleteAllInstantiatedGOs()
    {
        for (int i = gameObjectsInstantiated.Count - 1; i >= 0; i--)
        {
            DestroyImmediate(gameObjectsInstantiated[i]);
        }
    }

    private static void RotateItem(Vector3 startPos, Vector3 finalPos, GameObject go)
    {
        Vector3 lookPos = finalPos - startPos;
        var axis = go.transform.right;
        Quaternion lookRot = Quaternion.FromToRotation(axis, lookPos);
        go.transform.rotation = lookRot;
    }

    private static void MoveItem(GameObject go)
    {
        CircuitItem item = go.GetComponent<CircuitItem>();
        Vector3 offset = item.transform.position - item.initialNormal.position;
        go.transform.position += offset;
    }

    private static void ScaleItem(GameObject go, float currentSize, float targetSize)
    {
        Vector3 scale = go.transform.localScale;
        scale.x = targetSize * scale.x / currentSize;
        go.transform.localScale = scale;
    }
}
