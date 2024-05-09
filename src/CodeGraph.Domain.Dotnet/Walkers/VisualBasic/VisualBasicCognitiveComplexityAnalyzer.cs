using CodeGraph.Domain.Dotnet.Abstract;
using CodeGraph.Domain.Dotnet.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace CodeGraph.Domain.Dotnet.Walkers.VisualBasic
{
    /// <summary>
    ///     Visual Basic Cognitive Complexity Analysis
    /// </summary>
    public class VisualBasicCognitiveComplexityAnalyzer : VisualBasicSyntaxWalker, IDotnetComplexityAnalyzer
    {
        private readonly MethodBlockSyntax _methodBlockSyntax;
        private MethodStatementSyntax? _currentMethod;
        private bool _hasRecursion;
        private int _nesting;

        public VisualBasicCognitiveComplexityAnalyzer(MethodBlockSyntax methodBlockSyntax)
        {
            _methodBlockSyntax = methodBlockSyntax;
            Locations = new List<Location>();
            Visit(_methodBlockSyntax);
        }

        private IList<SyntaxNode> ToIgnore { get; } = new List<SyntaxNode>();

        public string MethodName => _methodBlockSyntax
            .SubOrFunctionStatement
            .Identifier
            .ToString();

        public string? ContainingClassName => _methodBlockSyntax
            .Ancestors()
            .OfType<ClassBlockSyntax>()
            .FirstOrDefault()?
            .ClassStatement
            .Identifier
            .ToString();

        public string? ContainingNamespace => _methodBlockSyntax
            .Ancestors()
            .OfType<NamespaceBlockSyntax>()
            .FirstOrDefault()?
            .NamespaceStatement
            .Name
            .ToString();

        public int ComplexityScore { get; private set; }

        public Location Location => _methodBlockSyntax.GetLocation();
        public List<Location> Locations { get; }

        public sealed override void Visit(SyntaxNode node)
        {
            base.Visit(node);
        }

        #region ForeachLoops

        public override void VisitForEachBlock(ForEachBlockSyntax node)
        {
            IncreaseComplexityByNesting(node.ForEachStatement.ForKeyword);
            VisitWithNesting(node, base.VisitForEachBlock);
        }

        #endregion

        #region For Loops

        public override void VisitForBlock(ForBlockSyntax node)
        {
            IncreaseComplexityByNesting(node.ForStatement.ForKeyword);
            VisitWithNesting(node, base.VisitForBlock);
        }

        #endregion

        #region While Loops

        public override void VisitWhileBlock(WhileBlockSyntax node)
        {
            IncreaseComplexityByNesting(node.WhileStatement.WhileKeyword);
            VisitWithNesting(node, base.VisitWhileBlock);
        }

        #endregion

        #region DoWhile Loops

        public override void VisitDoLoopBlock(DoLoopBlockSyntax node)
        {
            IncreaseComplexityByNesting(node.DoStatement.DoKeyword);
            VisitWithNesting(node, base.VisitDoLoopBlock);
        }

        #endregion

        #region Catch Clause

        public override void VisitCatchBlock(CatchBlockSyntax node)
        {
            IncreaseComplexityByNesting(node.CatchStatement.CatchKeyword);
            VisitWithNesting(node, base.VisitCatchBlock);
        }

        #endregion

        #region Goto (Considered harmful, ho ho ho)

        public override void VisitGoToStatement(GoToStatementSyntax node)
        {
            IncreaseComplexityByNesting(node.GoToKeyword);
            base.VisitGoToStatement(node);
        }

        #endregion

        #region Switch

        public override void VisitSelectBlock(SelectBlockSyntax node)
        {
            IncreaseComplexityByNesting(node.SelectStatement.SelectKeyword);
            VisitWithNesting(node, base.VisitSelectBlock);
        }

        public override void VisitCaseBlock(CaseBlockSyntax node)
        {
            IncreaseComplexity(node.CaseStatement.CaseKeyword);
            base.VisitCaseBlock(node);
        }

        #endregion

        #region Lambda

        public override void VisitSingleLineLambdaExpression(SingleLineLambdaExpressionSyntax node)
        {
            VisitWithNesting(node, base.VisitSingleLineLambdaExpression);
        }

        public override void VisitMultiLineLambdaExpression(MultiLineLambdaExpressionSyntax node)
        {
            VisitWithNesting(node, base.VisitMultiLineLambdaExpression);
        }

        #endregion

        #region Binary Expressions

        public override void VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            var nodeKind = node.Kind();

            if (!ToIgnore.Contains(node) && nodeKind is SyntaxKind.AndExpression or SyntaxKind.AndAlsoExpression
                    or SyntaxKind.OrExpression or SyntaxKind.OrElseExpression)
            {
                var left = node.Left.RemoveParentheses();
                if (!left.IsKind(nodeKind)) IncreaseComplexity(node.OperatorToken);

                var right = node.Right.RemoveParentheses();
                if (right.IsKind(nodeKind)) ToIgnore.Add(right);
            }

            base.VisitBinaryExpression(node);
        }

        public override void VisitBinaryConditionalExpression(BinaryConditionalExpressionSyntax node)
        {
            IncreaseComplexity(node.IfKeyword);
            VisitWithNesting(node, base.VisitBinaryConditionalExpression);
        }

        #endregion

        #region Complexity Modifiers

        private void IncreaseComplexity(SyntaxToken syntaxToken, int increment = 1)
        {
            ComplexityScore += increment;
            Locations.Add(syntaxToken.GetLocation());
        }

        private void IncreaseComplexityByNesting(SyntaxToken syntaxToken)
        {
            IncreaseComplexity(syntaxToken, _nesting + 1);
        }

        private void VisitWithNesting<TSyntaxNode>(TSyntaxNode syntaxNode, Action<TSyntaxNode> visitMethod)
        {
            _nesting++;
            visitMethod(syntaxNode);
            _nesting--;
        }

        #endregion

        #region If/Else Conditions

        public override void VisitSingleLineIfStatement(SingleLineIfStatementSyntax node)
        {
            IncreaseComplexity(node.IfKeyword, _nesting + 1);
            VisitWithNesting(node, base.VisitSingleLineIfStatement);
        }

        public override void VisitMultiLineIfBlock(MultiLineIfBlockSyntax node)
        {
            IncreaseComplexity(node.IfStatement.IfKeyword, _nesting + 1);
            VisitWithNesting(node, base.VisitMultiLineIfBlock);
        }

        public override void VisitElseIfStatement(ElseIfStatementSyntax node)
        {
            IncreaseComplexity(node.ElseIfKeyword);
            base.VisitElseIfStatement(node);
        }

        public override void VisitElseStatement(ElseStatementSyntax node)
        {
            IncreaseComplexity(node.ElseKeyword);
            base.VisitElseStatement(node);
        }

        public override void VisitTernaryConditionalExpression(TernaryConditionalExpressionSyntax node)
        {
            IncreaseComplexityByNesting(node.IfKeyword);
            VisitWithNesting(node, base.VisitTernaryConditionalExpression);
        }

        #endregion

        #region Methods

        public override void VisitMethodBlock(MethodBlockSyntax node)
        {
            _currentMethod = node.SubOrFunctionStatement;
            base.VisitMethodBlock(node);

            if (!_hasRecursion) return;

            _hasRecursion = false;
            IncreaseComplexity(node.SubOrFunctionStatement.Identifier);
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            if (node.Expression == null
                || node.ArgumentList == null
                || _currentMethod == null
                || node.ArgumentList.Arguments.Count != _currentMethod.ParameterList?.Parameters.Count)
                return;

            _hasRecursion = string.Equals(GetMethodName(node.Expression), _currentMethod.Identifier.ValueText,
                StringComparison.Ordinal);
            base.VisitInvocationExpression(node);
        }

        private static string GetMethodName(ExpressionSyntax expression)
        {
            if (expression.IsKind(SyntaxKind.IdentifierName))
                return (expression as IdentifierNameSyntax).Identifier.ValueText;

            if (expression.IsKind(SyntaxKind.SimpleMemberAccessExpression))
                return (expression as MemberAccessExpressionSyntax).Name.Identifier.ValueText;

            return string.Empty;
        }

        #endregion
    }
}