namespace LINQ
{
	internal class Program
	{
		static void Main(string[] args)
		{
			#region Session 01
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
			#endregion

			#region Session 02
			#region Partitioning Operators only in Method Syntax
			//int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
			#region Take - Skip
			//// First 3 numbers
			//var result = numbers.Take(3); // 5 4 1

			//// First 5 products by price desc
			//var result = ProductList.OrderByDescending(p => p.UnitPrice).Take(5);

			//// Skip first 4 numbers
			//var result = numbers.Skip(4); 
			#endregion

			#region TakeWhile - SkipWhile
			//// Take while less than 6
			//var result = numbers.TakeWhile(n => n < 6);

			//// Take while value >= position
			//var result = numbers.TakeWhile((n, i) => n >= i);

			//// Skip until divisible by 3
			//var result = numbers.SkipWhile(n => n % 3 != 0);

			//// Skip while value >= position
			//var result = numbers.SkipWhile((n, i) => n >= i); 
			#endregion

			#region Hybrid Syntax [Method Syntax + Query Syntax ]

			// (Query Syntax).Linq Operator 

			////// First 5 products by price desc
			////var result = ProductList.OrderByDescending(p => p.UnitPrice).Take(5);

			//var result = (from P in ProductList
			//			  orderby P.UnitPrice descending
			//			  select P).Take(5);

			#endregion

			#endregion

			#region Quantifiers Operators only in Method Syntax 
			//// Any out-of-stock product?
			//var result = ProductList.Any(p => p.UnitsInStock == 0);

			//// All Seafood products in stock?
			//bool allInStock = ProductList
			//				  .Where(p => p.Category == "Seafood")
			//				  .All(p => p.UnitsInStock > 0);

			//int[] nums = { 2, 3, 4, 5 };
			//Console.WriteLine("Contains(4): " + nums.Contains(4));
			//Console.WriteLine("Contains(9): " + nums.Contains(9));

			#endregion

			#region Let And Into [Valid With Query Syntax Only]


			//var Result = from P in ProductList
			//			 where P.UnitPrice * 0.9M < 10
			//			 select new
			//			 {
			//				 P.ProductName,
			//				 P.UnitPrice,
			//				 PriceAfterDisc = P.UnitPrice * 0.9M
			//			 };

			// Let => Continue Query With New Temp Variable 

			//Result = from P in ProductList
			//		 let DiscPrice = P.UnitPrice * 0.9M
			//		 where DiscPrice < 10
			//		 select new
			//		 {
			//			 P.ProductName,
			//			 P.UnitPrice,
			//			 PriceAfterDisc = DiscPrice
			//		 };
			//// Into => Restart Query With New Range Variable 
			//// Used With Group by 

			//var Result02 = from P in ProductList
			//			   select P.UnitPrice * 0.9M
			//			   into DiscPrice
			//			   where DiscPrice < 10
			//			   select DiscPrice;

			//foreach (var item in Result02)
			//	Console.WriteLine(item);

			#endregion

			#region Grouping

			#region Retrieves all products from ProductList That Out Of Stock , and groups the products by their Category

			//var byCategory = ProductList.Where(P => P.UnitsInStock == 0).GroupBy(P => P.Category);

			//byCategory = from P in ProductList
			//			 where P.UnitsInStock == 0
			//			 group P by P.Category;
			//foreach (var group in byCategory)
			//{
			//	Console.WriteLine($"    [{group.Key}]  ({group.Count()} products)");
			//	foreach (var product in group)
			//	{
			//		Console.WriteLine(product.ProductName);
			//	}
			//} 
			#endregion

			#region Example 02
			// Retrieves all products from ProductList That Out of Stock
			// Groups the products by their Category.
			// Filters out categories that contain 2 or fewer products.
			// Returns an anonymous object with two properties:
			// CategoryName 
			// CountOfProducts → the number of products in that category.

			//var result = ProductList.Where(P => P.UnitsInStock == 0)
			//						.GroupBy(P => P.Category)
			//						.Where(G => G.Count() <= 2)
			//						.Select(G => new
			//						{
			//							CategoryName = G.Key,
			//							CountOfProducts = G.Count()
			//						});
			//result = from p in ProductList
			//		 where p.UnitsInStock == 0
			//		 group p by p.Category
			//			 into Categories
			//		 where Categories.Count() <= 2
			//		 select new
			//		 {
			//			 CategoryName = Categories.Key,
			//			 CountOfProducts = Categories.Count()
			//		 };
			#endregion

			#region Group words by first letter (case-insensitive)
			//string[] w = { "Apple", "banana", "Avocado", "Berry", "Cherry", "apricot" };
			//var byLetter = w.GroupBy(s => s[0].ToString(), StringComparer.OrdinalIgnoreCase);
			//foreach (var g in byLetter)
			//	Console.WriteLine($"'{g.Key}': {string.Join(", ", g)}");
			#endregion

			#endregion

			#region Aggregation Operators only in Method Syntax
			// Count / LongCount / Sum / Min / Max / Average / Aggregate

			#region Count - CountLong
			//var productCount = ProductList.Count();
			//productCount = ProductList.Count;

			//var outOfStockCount = ProductList.Count(P => P.UnitsInStock == 0);
			//var outOfStockCountLong = ProductList.LongCount(P => P.UnitsInStock == 0);

			#endregion

			#region Sum

			//var total = ProductList.Sum(p => p.UnitsInStock);

			////  Total stock per category
			//var result = ProductList.GroupBy(p => p.Category)
			//					.Select(g => new { g.Key, TotalStock = g.Sum(p => p.UnitsInStock) })
			//					.OrderByDescending(x => x.TotalStock);

			#endregion

			#region Max - Min - Average
			//// Cheapest and most expensive product prices
			//var cheapestPrice = ProductList.Min(p => p.UnitPrice);
			//var highestPrice = ProductList.Max(p => p.UnitPrice);
			//var productWithHighestPrice = ProductList?.MaxBy(P => P.UnitPrice);
			//var productWithCheapestPrice = ProductList?.MaxBy(P => P.UnitPrice);

			//var productsAvgPrice = ProductList?.Average(p => p.UnitPrice);

			#endregion

			#region Aggregate
			//int[] numbers = [1, 2, 3, 4, 5];
			//int result = numbers.Aggregate((acc, n) => acc * n);
			//Console.WriteLine("1×2×3×4×5 = " + result);


			//// Comma-separated product names
			//string result = ProductList
			//			   .Take(5)
			//			   .Select(p => p.ProductName)
			//			   .Aggregate("", (acc, n) => acc == "" ? n : acc + ", " + n,
			//						 result => result.ToUpper());

			//Console.WriteLine(result);

			#endregion



			#endregion

			#region Set Operations Operators only in Method Syntax
			//// Distinct / Union / Intersect / Except
			//var categories = ProductList.Select(p => p.Category).Distinct();
			//string[] dup = { "apple", "Apple", "APPLE", "cherry", "Cherry" };
			//var withoutDup = dup.Distinct(StringComparer.OrdinalIgnoreCase);


			//int[] a = { 0, 2, 4, 5, 6, 8, 9 };
			//int[] b = { 1, 3, 5, 7, 8 };
			//string[] list1 = { "apple", "banana", "cherry" };
			//string[] list2 = { "Apple", "GRAPE", "CHERRY" };

			#region Union
			//var result = a.Union(b);

			//var prodLetters = ProductList.Select(p => p.ProductName[0].ToString());
			//var custLetters = CustomerList.Select(c => c.CompanyName[0].ToString());
			//var result = prodLetters.Union(custLetters, StringComparer.OrdinalIgnoreCase); 
			#endregion

			#region Intersect

			////var result = a.Intersect(b);

			//var result = list1.Intersect(list2, StringComparer.OrdinalIgnoreCase); 
			#endregion

			#region Except
			//var result = a.Except(b);


			//var result = list1.Except(list2, StringComparer.OrdinalIgnoreCase);

			#endregion

			#endregion

			#region Conversion
			//// ToArray / ToList / ToDictionary / Cast 


			//var array = ProductList.Select(p => p.ProductName).ToArray();

			//var list = ProductList.Where(p => p.Category == "Seafood").ToList();

			//var dic01 = ProductList.ToDictionary(p => p.ProductID);

			//var dic02 = ProductList.ToDictionary(p => p.ProductID, p => p.ProductName);

			//var dic03 = CustomerList.ToDictionary(c => c.CustomerID, c => c.CompanyName, StringComparer.OrdinalIgnoreCase);

			//ArrayList al = new() { 1, 2, 3, 4, 5 };
			//var result = al.Cast<int>();
			#endregion

			#region Element Operators Operators only in Method Syntax
			// First / FirstOrDefault / Last / LastOrDefault / Single / SingleOrDefault / ElementAt / ElementAtOrDefault
			#region First - FirstOrDefault 
			//var result = ProductList.First();
			//result = ProductList.Where(P => P.UnitPrice > 50m).First();
			//result = ProductList.First(p => p.UnitPrice > 50m);
			//result = ProductList.FirstOrDefault(p => p.Category == "Seafood");

			#endregion

			#region Last - LastOrDefault 
			//var result = ProductList.Last();
			//result = ProductList.Where(P => P.UnitPrice > 50m).Last();
			//result = ProductList.Last(p => p.UnitPrice > 50m);
			//result = ProductList.Last(p => p.Category == "Seafood");

			#endregion

			#region Single - SingleOrDefault
			//var result = ProductList.Single(p => p.ProductID == 13);
			////// If Sequence Contains No Element Matching Condition , Will Throw Exception 
			////// If Sequence Contains Just Only One Element Matching Condition , Will Return It 
			////// If Sequence Contains More Than One Element Matching Condition , Will Throw Exception 


			//result = ProductList.Single(p => p.ProductID == 13);
			////// If Sequence Contains No Element Matching Condition , Null 
			////// If Sequence Contains Just Only One Element Matching Condition , Will Return It 
			////// If Sequence Contains More Than One Element Matching Condition , Will Throw Exception 

			#endregion

			#region ElementAt - ElementAtOrDefault
			//var result = ProductList.ElementAt(4);
			//result = ProductList.ElementAtOrDefault(999);
			#endregion
			#endregion

			#region Generation Operators
			//var result = Enumerable.Range(1, 10);
			//var result = Enumerable.Range(0, 5).Select(n => n * n);
			//var result = Enumerable.Repeat("Hello", 4);
			//var result = Enumerable.Empty<Product>();
			//var result = ProductList.Where(p => p.ProductID == 999)
			//				 .DefaultIfEmpty(new Product { ProductName = "Unknown Product", UnitPrice = 0m });
			#endregion

			#region Sequence Operations
			//int[] a = { 1, 2, 3 };
			//int[] b = { 4, 5, 6 };

			//var result = a.Concat(b);
			//var result = ProductList.Select(p => p.ProductName)
			//				 .Concat(CustomerList.Select(c => c.CompanyName));

			//var result = a.Zip(b, (x, y) => x * y); 
			//int[] x1 = { 1, 2, 3 };
			//int[] x2 = { 1, 2, 3 };
			//int[] x3 = { 1, 2, 4 };
			//Console.WriteLine("x1 == x2: " + x1.SequenceEqual(x2));
			//Console.WriteLine("x1 == x3: " + x1.SequenceEqual(x3));

			//string[] s1 = { "Apple", "Banana" };
			//string[] s2 = { "apple", "BANANA" };
			//Console.WriteLine(s1.SequenceEqual(s2, StringComparer.OrdinalIgnoreCase));
			#endregion
			//foreach (var item in result)
			//	Console.WriteLine(item);
			#endregion
		}
	}
}
