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
			if (top <= 0) throw new InvalidOperationException("The stack is empty!");
			return array[top];
		}
		
		public int pop() {
			if (top <= 0) throw new InvalidOperationException("The stack is empty!");
			return array[top--];
		}
			                   
		public int count(){
			return (top + 1);
		}
	}
	
	[TestFixture]
	public class StackableTest {
		[Test]
		public void TestStackable() {
			try{
				new Stackable(0);
				Assert.Fail("'Stackable' should be thrown exceptions");
			} catch(ArgumentException){
			}
			
			Stackable s = new Stackable(2);
			s.push(1);
			s.peek();
			s.push(2);
			s.peek();
			
			try { s.push(3); } catch (InvalidOperationException) { }
			Assert.AreEqual(2, s.peek());
			
			Assert.AreEqual(2, s.pop());
			Assert.AreEqual(1, s.peek());
			
			Assert.AreEqual(1, s.pop());

			try { s.peek(); } catch (InvalidOperationException) { }
			try { s.pop(); } catch (InvalidOperationException) { }
		}
	}
}
	




