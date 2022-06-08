using Calc.SyntaxNodes;
using Calc.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    public class Parser
    {
        public SyntaxNode Parse(IEnumerable<Token> tokens)
        {
            return ParseExpression(new SavedPointsEnumerator<Token>(tokens.GetEnumerator()));
        }

        // expression = ( number | brackets ) arithmeticExpression?
        private SyntaxNode ParseExpression(SavedPointsEnumerator<Token> tokens)
        {
            var exp = ParseNumber(tokens) ?? ParseBrackets(tokens);
            exp = ParseArithmeticExpression(tokens, exp) ?? exp;

            return exp;
        }

        // arithmeticExpression = operator expression
        private ArithmeticOperation ParseArithmeticExpression(SavedPointsEnumerator<Token> tokens, SyntaxNode leftOperand)
        {
            tokens.CreateSavePoint();

            if (!tokens.MoveNext() || tokens.Current.Type != TokenType.Operator)
            {
                tokens.RestoreLastSavedPoint();
                return null;
            }

            var arithmeticExpression = new ArithmeticOperation(
                tokens.Current.Value == "+" ? Operator.Add :
                    tokens.Current.Value == "-" ? Operator.Substract :
                        tokens.Current.Value == "*" ? Operator.Multiply :
                            tokens.Current.Value == "/" ? Operator.Divide : throw new InvalidExpressionException())
            {
                LeftChild = leftOperand,
                RightChild = ParseExpression(tokens)
            };

            if (arithmeticExpression.RightChild == null)
            {
                tokens.RestoreLastSavedPoint();
                return null;
            }

            if ((arithmeticExpression.Operator == Operator.Multiply || arithmeticExpression.Operator == Operator.Divide) && !arithmeticExpression.RightChild.HasPriority && arithmeticExpression.RightChild is ArithmeticOperation otherArithmetic && (otherArithmetic.Operator == Operator.Add || otherArithmetic.Operator == Operator.Substract))
            {
                // opération de droite moins prioritaire, on "pivote"
                arithmeticExpression.RightChild = otherArithmetic.LeftChild;
                otherArithmetic.LeftChild = arithmeticExpression;
                arithmeticExpression = otherArithmetic;
            }

            tokens.RemoveLastSavedPoint();

            return arithmeticExpression;
        }

        // brackets = '(' expression ')'
        private SyntaxNode ParseBrackets(SavedPointsEnumerator<Token> tokens)
        {
            tokens.CreateSavePoint();

            if (tokens.MoveNext() && tokens.Current.Type == TokenType.LeftBracket)
            {
                var exp = ParseExpression(new SavedPointsEnumerator<Token>(new BetweenBracketEnumerator(tokens)));
                if (exp != null)
                {
                    exp.HasPriority = true;
                    return exp;
                }
            }

            tokens.RestoreLastSavedPoint();
            return null;
        }

        private SyntaxNode ParseNumber(SavedPointsEnumerator<Token> tokens)
        {
            tokens.CreateSavePoint();

            if (tokens.MoveNext() && tokens.Current.Type == TokenType.Number)
            {
                tokens.RemoveLastSavedPoint();
                return new Constant(int.Parse(tokens.Current.Value));
            }

            tokens.RestoreLastSavedPoint();
            return null;
        }

        // Itérateur d'une parenthèse à l'autre (ignorant les parenthèses imbriquées)
        private class BetweenBracketEnumerator : IEnumerator<Token>
        {
            private readonly IEnumerator<Token> _tokens;
            private int _openedBracket;

            public BetweenBracketEnumerator(IEnumerator<Token> tokens)
            {
                _tokens = tokens;
                _openedBracket = 1;
            }

            public Token Current => _tokens.Current;

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (!_tokens.MoveNext()) return false;

                if (_tokens.Current.Type == TokenType.LeftBracket) 
                    _openedBracket++;
                else if (_tokens.Current.Type == TokenType.RightBracket)
                {
                    _openedBracket--;
                    if (_openedBracket == 0) return false;
                }

                return true;
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }
    }
}
