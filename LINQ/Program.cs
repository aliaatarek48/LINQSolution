namespace LINQ
{
	internal class Program
	{
		static void Main(string[] args)
		{
			#region var vs dynamic

			//var name = "Salma";                  // compiler infers: string
			//name = "Mohamed";                    // still a string
			//									 //name = 99;                         // compile error — cannot convert int to string

			//Console.WriteLine(name.GetType());   // System.String
			//Console.WriteLine(name.Length);

			////var x;                             // compile error — must be initialized at declaration
			////var y = null;                      // compile error — compiler cannot infer a type from null


			//dynamic value;                       // no initialization needed
			//value = 42;
			//Console.WriteLine(value.GetType());  // System.Int32

			//value = true;
			//Console.WriteLine(value.GetType());  // System.Boolean

			//value = new Customer { CustomerName = "Ali" };
			//Console.WriteLine(value.GetType());  // Customer

			//Console.WriteLine(value.CustomerName);   // resolved at runtime 

			//value = "Ahmed";
			//Console.WriteLine(value.CustomerName);   // compiles fine , resolved at runtime throws RuntimeBinderException at runtime

			#endregion

			#region Anonymous Types

			//var employee1 = new { Id = 1, Name = "Ahmed", Salary = 5000 };

			//Console.WriteLine(employee1);            // ToString() is overridden → { Id = 1, Name = Ahmed, Salary = 5000 }
			//Console.WriteLine(employee1.GetType());  // compiler-generated name, e.g. <>f__AnonymousType0`3[...]


			//var employee2 = new { Id = 2, Name = "Ahmed", Salary = 6000 };
			//// Same property names + same property types + same ORDER → compiler reuses one type
			//Console.WriteLine(employee1.GetType() == employee2.GetType());   // True — same type, different values

			//// Different property names (case-sensitive) → different type
			//var employee3 = new { id = 4, name = "Omar", salary = 7000 };
			//Console.WriteLine(employee1.GetType() == employee3.GetType());   // False

			//// Same names but different order → different type
			//var employee4 = new { Id = 3, Salary = 7000, Name = "Omar" };
			//Console.WriteLine(employee1.GetType() == employee4.GetType());   // False

			//// ===== Equality =====
			//// Equals() and GetHashCode() are overridden → compare by VALUES
			//// == is not overridden → compares REFERENCES

			//var employee1Copy = new { Id = 1, Name = "Ahmed", Salary = 5000 };
			//Console.WriteLine(employee1.Equals(employee1Copy));   // True  — same values
			//Console.WriteLine(employee1 == employee1Copy);        // False — two different objects in memory

			//// ===== Immutability =====
			//// All properties are read-only (get only)
			////employee1.Name = "Ali";   // compile error — property is read-only

			//// To "change" a value, we create a NEW object:

			//// Way 1: projection initializers — property names are taken from the source (Id, Salary)
			//var employee5 = new { employee4.Id, employee4.Salary, Name = "Salma" };
			//Console.WriteLine(employee5);
			//Console.WriteLine(employee5.GetType() == employee4.GetType());   // True — same names, types and order

			//// Way 2: 'with' expression (C# 10+) — copies the object and changes only what we specify
			//var employee6 = employee4 with { Name = "Salma" };
			//Console.WriteLine(employee6);
			//Console.WriteLine(employee4);            // original object is unchanged


			#endregion

			#region Query Syntax vs Method Syntax

			//var students = new List<Student>
			//                      {
			//                      	new() { Name = "Sara",  Grade = 90, Major = "CS"   },
			//                      	new() { Name = "Omar",  Grade = 75, Major = "Math" },
			//                      	new() { Name = "Layla", Grade = 88, Major = "CS"   },
			//                      	new() { Name = "Karim", Grade = 60, Major = "CS"   },
			//                      	new() { Name = "Nour",  Grade = 95, Major = "Math" },
			//                      };

			///*
			//    Select  s.Name , s.Grade
			//    from Students s 
			//    where s.Grade >= 80 and s.Major = 'CS'
			//    Order By s.Grade Desc
			// */			

			//// ===== Query Syntax (Query Expression) =====
			//// SQL-like keywords. Must start with 'from' and end with 'select' or 'group by'.

			//var queryResult = from s in students                        // s = range variable (each student)
			//				  where s.Grade >= 80 && s.Major == "CS"    // filter
			//				  orderby s.Grade descending                // sort
			//				  select new { s.Name, s.Grade };           // project into an anonymous type

			//// ===== Method Syntax (Fluent Syntax) =====
			//// Extension methods chained together, each one takes a lambda expression.

			//var methodResult = students
			//	.Where(s => s.Grade >= 80 && s.Major == "CS")           // filter
			//	.OrderByDescending(s => s.Grade)                        // sort
			//	.Select(s => new { s.Name, s.Grade });                  // project into an anonymous type

			//// Both are the SAME: query syntax is syntactic sugar,
			//// the compiler translates it into the method calls above.

			//Console.WriteLine("Query Syntax:");
			//foreach (var item in queryResult)
			//	Console.WriteLine($"{item.Name} => {item.Grade}");

			//Console.WriteLine("Method Syntax:");
			//foreach (var item in methodResult)
			//	Console.WriteLine($"{item.Name} => {item.Grade}");

			//// Output (same for both):
			//// Sara => 90
			//// Layla => 88
			#endregion

			#region Deferred vs Immediate Execution

			//var numbers = new List<int> { 1, 2, 3, 4, 5 };

			//// ===== DEFERRED Execution =====

			//var result01 = numbers.Where(n => n % 2 == 0);

			//numbers.Add(6);                           // list changed AFTER defining the query

			//// Execution happens HERE, when we enumerate the query
			//foreach (var num in result01)
			//	Console.WriteLine(num);               // 2 4 6 — sees the 6, because it runs now

			//// The query runs again from scratch on EVERY enumeration
			//numbers.Add(10);
			//foreach (var num in result01)
			//	Console.WriteLine(num);               // 2 4 6 10

			//// ===== IMMEDIATE Execution =====

			//var result02 = numbers.Where(n => n % 2 == 0).ToList();

			//numbers.Add(8);                           // list changed AFTER execution
			//foreach (var num in result02)
			//	Console.WriteLine(num);               // 2 4 6 10 — no 8, the snapshot was already taken

			//// ===== Which operators are which? =====
			//// Deferred:  operators that return IEnumerable<T>
			////            Where, Select, OrderBy, Take, Skip, Distinct ...
			//// Immediate: operators that return a single value or a ready collection
			////            ToList, ToArray, ToDictionary, Count, Sum, Max, First, Single, Any ...
			#endregion

			#region Filtering - Restriction

			#region Where

			//// Out of stock products
			//var outOfStock = ProductList.Where(p => p.UnitsInStock == 0);       // method syntax

			//outOfStock = from p in ProductList                                  // query syntax — same result
			//			 where p.UnitsInStock == 0
			//			 select p;

			//foreach (var product in outOfStock)
			//	Console.WriteLine(product);

			//// In stock AND price > $20 — combine conditions with && (or ||)
			//var inStockExpensive = ProductList.Where(p => p.UnitsInStock > 0 && p.UnitPrice > 20m);

			//inStockExpensive = from p in ProductList
			//				   where p.UnitsInStock > 0 && p.UnitPrice > 20m
			//				   select p;

			//foreach (var product in inStockExpensive)
			//	Console.WriteLine(product);
			#endregion

			#region Indexed Where
			//// Overload that also gives us the INDEX of each element (zero-based position in the source).
			//// Available in method syntax ONLY — no query syntax equivalent.

			//// Out of stock products among the first 20 products (index 0 → 19)
			//var outOfStockInFirst20 = ProductList.Where((p, i) => p.UnitsInStock == 0 && i < 20);

			//foreach (var product in outOfStockInFirst20)
			//	Console.WriteLine(product);
			#endregion

			#region OfType<>
			//// Filters elements by their TYPE and returns IEnumerable<T>.
			//// where Where() is not available because the elements are stored as object.

			//ArrayList mixed = new() { 1, "hello", 3.14, "world", true, "linq" };

			//var strings = mixed.OfType<string>();    // keeps strings only, skips everything else

			//foreach (var str in strings)
			//	Console.WriteLine(str);              // hello world linq

			////var casted = mixed.Cast<string>().ToList();   // runtime error
			//// OfType<T>() vs Cast<T>():
			//// OfType skips elements that are not T,
			//// Cast throws InvalidCastException on the first element that is not T.
			#endregion

			#endregion

			#region Projection

			#region Select

			//var productNames = ProductList.Select(p => p.ProductName);          // method syntax

			//productNames = from p in ProductList                                // query syntax — same result
			//			   select p.ProductName;

			//foreach (var name in productNames)
			//	Console.WriteLine(name);

			//// Project into an anonymous type: { ProductName, Category, Price }
			//var productInfo = ProductList.Select(p => new { p.ProductName, p.Category, Price = p.UnitPrice });

			//foreach (var item in productInfo)
			//	Console.WriteLine(item);

			//// Indexed overload — also gives the zero-based index of each element.
			//// Method syntax ONLY — no query syntax equivalent.
			//int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

			//// InPlace = true when the number's value equals its position in the array
			//var numbersInPlace = numbers.Select((n, i) => new { Num = n, InPlace = n == i });

			//foreach (var item in numbersInPlace)
			//	Console.WriteLine(item);             // InPlace is True for 3, 6 and 7 only

			#endregion

			#region SelectMany
			//// With Select: we get a collection of collections (one orders collection per customer)
			//var ordersPerCustomer = CustomerList.Select(c => c.Orders);

			//foreach (var customerOrders in ordersPerCustomer)                   // needs a nested loop
			//	foreach (var order in customerOrders)
			//		Console.WriteLine(order);

			//// With SelectMany: all orders flattened into ONE sequence
			//// (customers with no orders simply produce nothing)
			//var allOrders = CustomerList.SelectMany(c => c.Orders);

			//foreach (var order in allOrders)                                    // single loop
			//	Console.WriteLine(order);

			//// SelectMany with result selector:
			//// the second lambda receives BOTH the parent (c) and the child (o),
			//// so we can keep customer data next to each order
			//var customerOrdersInfo = CustomerList.SelectMany(c => c.Orders,
			//												 (c, o) => new { c.CustomerID, o.OrderID, o.Total });

			//foreach (var item in customerOrdersInfo)
			//	Console.WriteLine(item);

			//// Indexed SelectMany — ci is the customer's index
			//var ordersWithCustomerIndex = CustomerList.SelectMany((c, ci) => c.Orders
			//	.Select(o => $"Customer #{ci + 1} [{c.CustomerID}] => Order Id: {o.OrderID}"));

			//// Indexed on both levels — oi is the order's index and restarts from 0 for each customer
			//var ordersWithBothIndexes = CustomerList.SelectMany((c, ci) => c.Orders
			//	.Select((o, oi) => $"Customer #{ci + 1} [{c.CustomerID}] => Order #{oi + 1} Order Id: {o.OrderID}"));

			//foreach (var line in ordersWithBothIndexes)
			//	Console.WriteLine(line);

			//// Query syntax: multiple 'from' clauses = SelectMany
			//// Orders with Total < $500
			//var cheapOrders = from c in CustomerList
			//				  from o in c.Orders                                // second from → SelectMany
			//				  where o.Total < 500m
			//				  select new { c.CustomerID, o.OrderID, o.Total };

			//// Same query in method syntax
			//var cheapOrdersMethod = CustomerList
			//	.SelectMany(c => c.Orders, (c, o) => new { c.CustomerID, o.OrderID, o.Total })
			//	.Where(x => x.Total < 500m);

			//foreach (var item in cheapOrders)
			//	Console.WriteLine(item);

			#endregion

			#endregion

			#region Ordering

			//var result = ProductList.Order(); // InvalidOperationException
			//								  //  At least one object must implement IComparable.
			//foreach (var item in result)
			//{
			//	Console.WriteLine(item);
			//}


			//var byName = ProductList.OrderBy(p => p.ProductName);               // A → Z

			//var byPriceDesc = ProductList.OrderByDescending(p => p.UnitPrice);  // highest price first

			//foreach (var product in byPriceDesc)
			//	Console.WriteLine(product);

			//// ===== Ordering with a comparer =====
			//// The second parameter controls HOW the keys are compared.
			//string[] words = { "aPPLE", "AbAcUs", "bRaNcH", "BlUeBeRrY", "ClOvEr", "cHeRry" };

			//// Default (no comparer) = StringComparer.CurrentCulture → linguistic order
			//var byDefault = words.OrderBy(w => w);
			//// AbAcUs, aPPLE, BlUeBeRrY, bRaNcH, cHeRry, ClOvEr

			//// OrdinalIgnoreCase → same output for these words
			//var ignoreCase = words.OrderBy(w => w, StringComparer.OrdinalIgnoreCase);
			//// AbAcUs, aPPLE, BlUeBeRrY, bRaNcH, cHeRry, ClOvEr

			//// Ordinal (must be passed explicitly) → compares character codes, uppercase first
			//var ordinal = words.OrderBy(w => w, StringComparer.Ordinal);
			//// AbAcUs, BlUeBeRrY, ClOvEr, aPPLE, bRaNcH, cHeRry

			//foreach (var word in ordinal)
			//	Console.WriteLine(word);


			//// ===== ThenBy / ThenByDescending =====
			//// Secondary sort: applied only between elements that are EQUAL in the first key.

			//// By category, and inside each category by name
			//var byCategoryThenName = ProductList.OrderBy(p => p.Category)
			//									.ThenBy(p => p.ProductName);

			//// By category, and inside each category by price (highest first)
			//var byCategoryThenPrice = ProductList.OrderBy(p => p.Category)
			//									 .ThenByDescending(p => p.UnitPrice);

			//// Same query in query syntax: comma = ThenBy, 'ascending' is the default
			//byCategoryThenPrice = from p in ProductList
			//					  orderby p.Category, p.UnitPrice descending
			//					  select p;

			//foreach (var product in byCategoryThenPrice)
			//	Console.WriteLine(product);


			//// ===== Reverse =====
			//// Does NOT sort — just flips the current order of the sequence (no comparison).
			//// Method syntax ONLY — no query syntax equivalent.
			//string[] names = { "Aya", "omar", "Amr", "Mohamed", "Ahmed" };

			//var reversed = names.Reverse();
			//// Ahmed, Mohamed, Amr, omar, Aya

			//foreach (var name in reversed)
			//	Console.WriteLine(name);

			#endregion

			#region Quantifiers Operators only in Method Syntax
			//// Quantifiers answer a yes/no question about the sequence → they return bool.
			//// Immediate execution (they return a value, not a query).
			//// Method syntax ONLY — no query syntax keywords.

			//// ===== Any =====
			//// Any(): is there at least ONE element? (is the sequence not empty?)
			//bool hasProducts = ProductList.Any();
			//Console.WriteLine($"Has products: {hasProducts}");
			//// Prefer Any() over Count() > 0 — Any stops at the first element, Count goes through all of them

			//// Any(predicate): does at least ONE element satisfy the condition?
			//// Stops at the first match.
			//bool anyOutOfStock = ProductList.Any(p => p.UnitsInStock == 0);
			//Console.WriteLine($"Any out of stock: {anyOutOfStock}");

			//// ===== All =====
			//// All(predicate): do ALL elements satisfy the condition?
			//// Stops at the first element that fails.

			//// Are all Seafood products in stock?
			//bool allInStock = ProductList
			//				  .Where(p => p.Category == "Seafood")
			//				  .All(p => p.UnitsInStock > 0);
			//Console.WriteLine($"All seafood in stock: {allInStock}");

			//// Watch out: All returns TRUE on an empty sequence (no element fails the condition)
			//bool allOnEmpty = ProductList
			//				  .Where(p => p.Category == "NoSuchCategory")
			//				  .All(p => p.UnitsInStock > 0);
			//Console.WriteLine($"All on empty: {allOnEmpty}");               // True

			//// ===== Contains =====
			//// Contains(value): does the sequence contain this specific element?
			//// Uses Equals for comparison: by value for int/string,
			//// by REFERENCE for classes (unless Equals is overridden or a comparer is passed).
			//int[] nums = { 2, 3, 4, 5 };
			//Console.WriteLine($"Contains(4): {nums.Contains(4)}");          // True
			//Console.WriteLine($"Contains(9): {nums.Contains(9)}");          // False

			//// Contains(value, comparer): control how elements are compared
			//string[] fruits = { "Apple", "Banana", "Cherry" };

			//Console.WriteLine(fruits.Contains("cherry"));                                       // False — case-sensitive
			//Console.WriteLine(fruits.Contains("cherry", StringComparer.OrdinalIgnoreCase));     // True

			#endregion
		}
	}
}
