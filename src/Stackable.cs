using System;
using NUnit.Framework;

namespace oishi.com
{
	public class Stackable{
		int top;
		int[] array;
		
		public Stackable(int capacity){
			if ( capacity <= 0 ) throw new ArgumentException ("'capacity' must be positive");
			array = new int[capacity];
			top = -1;
		}
		
		public void push(int i){
			if (array.Length -1 == top) throw new InvalidOperationException("The stack is full!");
			top++;
			array[top] = i;
			
		}
		
		public int peek() {
			if (top < 0) throw new InvalidOperationException("The stack is empty!");
			return array[top];
		}
		
		public int pop() {
			if (top < 0) throw new InvalidOperationException("The stack is empty!");
			return array[top--];
		}
			                   
		public int count(){
			return (top + 1);
		}
	}
	
	public class Queueable {
		int[] array;
		int head;
		int tail;
		
		public Queueable(int capacity) {
			if (capacity <= 0) throw new ArgumentException("'capacity' must be positive");
			head = -1;
			tail = -1;
			array = new int[capacity];
		}
		
		public void enqueue(int n){
			if (array.Length - 1 == tail) throw new InvalidOperationException("'The queue is full");
			if (head < 0) head = 0;
			tail++;
			array[tail] = n;
		}
		
		public int dequeue() {
			if (head > tail || head < 0) throw new InvalidOperationException("'The queue is empty");
			return array[head++];
		}
		
		public int count() {
			return (tail - head + 1);
		}
	}
	
	[TestFixture]
	public class StackableTest {
		[Test]
		public void TestStackable() {
			
			try{
				new Stackable(0);
				Assert.Fail("'Stackable' should be thrown an exception.");
			} catch(ArgumentException){
			}
			
			Stackable s = new Stackable(2);
			s.push(1);
			s.peek();
			s.push(2);
			s.peek();
			
			try {
				s.push(3);
				Assert.Fail("'Stackable' should be thrown an exception.");
			} catch (InvalidOperationException) { 
			}
			Assert.AreEqual(2, s.count());
			Assert.AreEqual(2, s.peek());
			
			Assert.AreEqual(2, s.pop());
			Assert.AreEqual(1, s.count());
			Assert.AreEqual(1, s.peek());
			
			Assert.AreEqual(1, s.pop());
			Assert.AreEqual(0, s.count());
			try { 
				s.peek(); 
				Assert.Fail("'Stackable' should be thrown an exception.");
			} catch (InvalidOperationException) { 
			}
			try { 
				s.pop(); 
				Assert.Fail("'Stackable' should be thrown an exception.");
			} catch (InvalidOperationException) { 
			}
		}
	}
	
	[TestFixture]
	public class QueueableTest {
		[Test]
		public void TestQueueable() {
			
			try{
				new Stackable(0);
				Assert.Fail("'Queueable' should be thrown an exception.");
			} catch(ArgumentException){
			}
			
			Queueable q = new Queueable(2);
			q.enqueue(1);
			q.enqueue(2);
			
			try {
				q.enqueue(3);
				Assert.Fail("'Queueable' should be thrown an exception.");
			} catch (InvalidOperationException) { 
			}
			Assert.AreEqual(1, q.dequeue());
			
			Assert.AreEqual(1, q.count());
			Assert.AreEqual(2, q.dequeue());
			
			Assert.AreEqual(0, q.count());
			try { 
				q.dequeue(); 
				Assert.Fail("'Stackable' should be thrown an exception.");
			} catch (InvalidOperationException) { 
			}
		}
	}
}
	




