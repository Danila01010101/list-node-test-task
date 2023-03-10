using System.IO;

public class ListRand
{
    public ListNode Head;
    public ListNode Tail;
    public int Count;

    private ListNode GetNode(int index)
    {
        int counter = 0;
        for (ListNode currentNode = Head; currentNode != null; currentNode = currentNode.Next)
        {
            if (counter == index)
                return currentNode;
            counter++;
        }
        return new ListNode();
    }

    public void Serialize(FileStream s)
    {
        Dictionary<ListNode, int> randomNodeIndexes = new Dictionary<ListNode, int>();
        int indexCounter = 0;
        Head.Rand = null;

        for (ListNode currentNode = Head; currentNode != null; currentNode = currentNode.Next)
        {
            if (currentNode.Rand != null)
                randomNodeIndexes.Add(currentNode, indexCounter);
            else
                randomNodeIndexes.Add(currentNode, -1);
            indexCounter++;
        }

        using (StreamWriter streamReader = new StreamWriter(s))
        {
            for (ListNode currentNode = Head; currentNode != null; currentNode = currentNode.Next)
            {
                string result = currentNode.Data + ":";
                if (currentNode.Rand != null)
                {
                    streamReader.Write(result + randomNodeIndexes[currentNode.Rand] + ";");
                }
                else
                {
                    streamReader.Write(result + "-1;");
                }
            }
        }
    }

    public void Deserialize(FileStream s)
    {
        try
        {
            string[] splittedText = new StreamReader(s).ReadToEnd().Split(";");

            Count = splittedText.Length - 1;

            int[] randomNodeIndexes = new int[Count];
            ListNode currentNode = new ListNode();

            currentNode = new ListNode();
            string text = splittedText[0];
            var values = text.Split(":");
            var data = values[0];
            int index = int.Parse(values[1]);
            randomNodeIndexes[0] = index;
            currentNode.Data = data;

            Head = currentNode;

            for (int i = 1; i < Count; i++)
            {
                text = splittedText[i];
                values = text.Split(":");
                data = values[0];
                index = int.Parse(values[1]);

                var node = new ListNode();
                currentNode.Next = node;
                node.Data = data;
                node.Prev = currentNode;
                currentNode = node;
                currentNode.Data = data;
                randomNodeIndexes[i] = index;
            }

            Tail = currentNode;

            for (int i = 0; i < Count; i++)
            {
                if (randomNodeIndexes[i] != -1)
                    GetNode(i).Rand = GetNode(randomNodeIndexes[i]);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Не удалось считать файл:");
            Console.WriteLine(e.Message);
        }
    }
}