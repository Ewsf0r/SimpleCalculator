namespace CalculatorTestAppService.Interfaces.Operation
{
    public interface IOperation
    {
        string[] Operators { get; }
        int Order { get; }
        bool CanParse(string operatorStr);
        IOperation? Parse(string expressionStr, int opPosition);
        double GetResult();
        bool CheckExpression(string expressionStr) => true;
    }
}
