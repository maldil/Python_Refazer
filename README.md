# Python_Refazer
Python version of the Refazer 

Dependencies : 1. IronPython, 2. Microsoft.ProgramSynthesis

Example = https://github.com/maldil/Python_Refazer/blob/main/Python_Refazer/src/Program.cs#L18

```
for pos in pronounce:
    count = count + pos
=====>
np.sum(pronounce)
```
    
```
Replace:
  CurrentStep  => kind = "DummyBlock"
  ForStatement  
  |---NameExpression,2  => value = "pronounce" && kind = "NameExpression"
  |---NameExpression  => value = "pos"
  |---SuiteStatement  
      |---AssignmentStatement  
          |---NameExpression  => value = "count"
          |---operator  => value = "Add"
              |---NameExpression,2  => value = "pos" && kind = "NameExpression"
              |---NameExpression  => value = "count"
With:
  Node: DummyBlock.value = ""
    Node: ExpressionStatement.value = ""
      Node: function call.value = ""
        Node: MemberExpression.value = "sum"
          LeafNode: NameExpression.value = "np"
        Node: Arg.value = ""
          SelectNode #0
```          
