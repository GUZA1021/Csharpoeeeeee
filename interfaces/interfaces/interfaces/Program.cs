namespace interfaces
{
    class Program
    {
        static void Main(string[] args)
        {
            Animal kangas = new Cow();
            Animal hund = new Dog();
            Animal kat = new Cat();
            Animal s = new Cow();
            Animal d = new Dog();
            Animal f = new Cat();
            Animal a = new Cow();
            Animal h = new Dog();
            Animal q = new Cat();
            List<Animal> list = new List<Animal> {kangas,hund,kat,s,d,f,a,h,q};
            foreach (Animal i in list)
            {
                i.AnimalSound();
            }
        }
    }

    interface Animal
    {
        void AnimalSound();
    }

    class Cow : Animal
    {
        public void AnimalSound()
        {
            Console.WriteLine("Kangas siger mooo");
        }
    }

    class Dog : Animal
    {
        public void AnimalSound()
        {
            Console.WriteLine("woof woof");
        }
    }

    class Cat : Animal
    {
        public void AnimalSound()
        {
            Console.WriteLine("meow meow");
        }
    }

}
