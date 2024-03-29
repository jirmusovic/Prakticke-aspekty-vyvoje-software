﻿/**
 * @file Evaluation.cs
 * 
 * @brief Třída pro vyhodnocení matematických výrazů
 * 
 * @author xnovak3g
 * 
 * @date 28-04-2022
 */

using System;
using System.Data;

namespace Calc
{
    /**
     * @class Třída pro vyhodnocení matematických výrazů
     */
    public class Evaluation
    {
        /**
         * @brief Funkce, která vrací výsledek výrazu v textovém řetězci
         * 
         * @param expression Textový řetězec pro vyhodnocení
         * @return Textový řetězec obsahující výsledek, null v případě chyby
         */
        public string Evaluate(string expression)
        {
            expression = expression.Replace(" ", String.Empty); // Odstranění bílých znaků
            double res = Eval(expression); /**Výsledek*/
            if(double.IsNaN(res))
            {
                return null;
            }
            return res.ToString();
        }

        /**
         * @brief   Funkce na vyhodnocení výrazu. Funkce pracuje na principu stálého zjednodušování problému. 
         *          Při nalezení podvýrazu se tato část problému vyhodnotí a jejím výsledkem se nahradí její původní výskyt v řetězci
         * @param expression Textový řetězec pro vyhodnocení
         * @return Výsledek výrazu, double.NaN v případě chyby výpočtu
         */
        private double Eval(string expression)
        {
            expression = RemoveNotNecessaryBrackets(expression); //Odstanění přebytečných závorek okolo řetězce

            if (expression.IndexOf("sin()") >= 0 || expression.IndexOf("cos()") >= 0 || expression.IndexOf("tan()") >= 0) //Ověření, jestli řetězec neobsahuje goniometrické funkce bez parametru
                return double.NaN;

            int firstStringPosition; /**Index prvního výskytu podvýrazu/funkce*/

            while ((firstStringPosition = expression.IndexOf('!')) >= 0) //Cyklus pro odhalení a nahrazení všech výskytů faktoriálu
            {
                int lastStringPosition = firstStringPosition;
                firstStringPosition = GetSubstringPos(expression, firstStringPosition - 1, false);
                if (expression[firstStringPosition] == '(' && expression[lastStringPosition - 1] != ')')
                    firstStringPosition++;
                string found = expression.Substring(firstStringPosition, lastStringPosition - firstStringPosition + 1);
                string result = Factorial(found);
                if (result == null)
                    return double.NaN;
                expression = expression.Replace(found, result.Replace(',', '.'));
            }

            while ((firstStringPosition = expression.IndexOf('^')) >= 0) //Cyklus pro odhalení a nahrazení všech výskytů faktoriálu
            {
                int midPos = firstStringPosition;
                int lastStringPosition = GetSubstringPos(expression, midPos + 1, true);
                firstStringPosition = GetSubstringPos(expression, midPos - 1, false);
                string found = expression.Substring(firstStringPosition, lastStringPosition - firstStringPosition + 1);
                string result = N_power(found);
                if (result == null)
                    return double.NaN;
                expression = expression.Replace(found, result.Replace(',', '.'));
            }

            while ((firstStringPosition = expression.IndexOf("f(")) >= 0) //Cyklus pro odhalení a nahrazení všech výskytů odmocnin
            {
                int lastStringPosition = GetSubstringPos(expression, firstStringPosition, true);
                string found = expression.Substring(firstStringPosition, lastStringPosition - firstStringPosition + 1);
                string result = N_root(found);
                if (result == null)
                    return double.NaN;
                expression = expression.Replace(found, result.Replace(',', '.'));
            }
            while ((firstStringPosition = expression.IndexOf("log(")) >= 0) //Cyklus pro odhalení a nahrazení všech výskytů dekadických logaritmů
            {
                int lastStringPosition = GetSubstringPos(expression, firstStringPosition, true);
                string found = expression.Substring(firstStringPosition, lastStringPosition - firstStringPosition + 1);
                string result = Log(found);
                if (result == null)
                    return double.NaN;
                expression = expression.Replace(found, result.Replace(',', '.'));
            }
            while ((firstStringPosition = expression.IndexOf("sin(")) >= 0) //Cyklus pro odhalení a nahrazení všech výskytů sin
            {
                int lastStringPosition = GetSubstringPos(expression, firstStringPosition, true);
                string found = expression.Substring(firstStringPosition, lastStringPosition - firstStringPosition + 1);
                string result = Sin(found);
                if (result == null)
                    return double.NaN;
                expression = expression.Replace(found, result.Replace(',', '.'));
            }
            while ((firstStringPosition = expression.IndexOf("cos(")) >= 0) //Cyklus pro odhalení a nahrazení všech výskytů cos
            {
                int lastStringPosition = GetSubstringPos(expression, firstStringPosition, true);
                string found = expression.Substring(firstStringPosition, lastStringPosition - firstStringPosition + 1);
                string result = Cos(found);
                if (result == null)
                    return double.NaN;
                expression = expression.Replace(found, result.Replace(',', '.'));
            }
            while ((firstStringPosition = expression.IndexOf("tan(")) >= 0) //Cyklus pro odhalení a nahrazení všech výskytů tan
            {
                int lastStringPosition = GetSubstringPos(expression, firstStringPosition, true);
                string found = expression.Substring(firstStringPosition, lastStringPosition - firstStringPosition + 1);
                string result = Tan(found); 
                if (result == null)
                    return double.NaN;
                expression = expression.Replace(found, result.Replace(',', '.'));
            }
            while ((firstStringPosition = expression.IndexOf("π")) >= 0) //Cyklus pro odhalení a nahrazení všech výskytů zanku π
            {
                string pi = Math.PI.ToString().Replace(',', '.');
                if(firstStringPosition != 0) //Doplnění znaku * před π, pokud nebyl zadán uživatelem a je nutný pro další výpočty
                {
                    char tmp = expression[firstStringPosition - 1];
                    if (!(tmp == '*' || tmp == '/' || tmp == '-' || tmp == '+' || tmp == '(' || tmp == '^'))
                        pi = "*" + pi;
                }
                if (firstStringPosition != expression.Length-1) //Doplnění znaku * za π, pokud nebyl zadán uživatelem a je nutný pro další výpočty
                {
                    char tmp = expression[firstStringPosition +1];
                    if (!(tmp == '*' || tmp == '/' || tmp == '-' || tmp == '+' || tmp == ')' || tmp == '^'))
                        pi = pi + "*";
                }
                expression = expression.Replace(expression.Substring(firstStringPosition, 1), pi);

            }
            while ((firstStringPosition = expression.IndexOf("e")) >= 0) //Cyklus pro odhalení a nahrazení všech výskytů zanku e
            {
                string e = Math.E.ToString().Replace(',', '.'); 
                if (firstStringPosition != 0) //Doplnění znaku * před e, pokud nebyl zadán uživatelem a je nutný pro další výpočty
                {
                    char tmp = expression[firstStringPosition - 1];
                    if (!(tmp == '*' || tmp == '/' || tmp == '-' || tmp == '+' || tmp == '(' || tmp == '^'))
                        e = "*" + e;
                }
                if (firstStringPosition != expression.Length - 1) //Doplnění znaku * za e, pokud nebyl zadán uživatelem a je nutný pro další výpočty
                {
                    char tmp = expression[firstStringPosition + 1];
                    if (!(tmp == '*' || tmp == '/' || tmp == '-' || tmp == '+' || tmp == ')' || tmp == '^'))
                        e = e + "*";
                }
                expression = expression.Replace(expression.Substring(firstStringPosition, 1), e);

            }

            return StringToNum(expression);
        }

        /**
         * @brief Pomocná funkce pro finální vyhodnocení výrazu po odstranění speciálních funkcí
         * 
         * @param expression Textový řetězec pro vyhodnocení
         * @return Výsledek výrazu, double.NaN v případě chyby výpočtu
         */
        private double StringToNum(string expression)
        {
            expression = expression.Replace(',', '.');
            DataTable table = new DataTable();
            double res;
            try
            {
                res = Convert.ToDouble(table.Compute(expression, ""));
            }
            catch
            {
                res = double.NaN;
            }
            if (double.IsInfinity(res))
                return double.NaN;
            return res;
        }

        /**
         * @brief Funkce pro výpočet n-té odmocniny čísla x
         * 
         * @param expression Výraz ve tvaru f(x,n), který má být vyhodnocen
         * @return Výsledek výrazu, null v případě chyby výpočtu nebo tvaru
         */
        public string N_root(string expression)
        {
            expression = expression.Replace(" ", String.Empty); // Odstranění bílých znaků

            double x = 0;
            double n = 0;

            char[] startTrim = new char[] { 'f', '(' }; /**Znaky, které mají být odstaněny ze začátku řetězce*/

            expression = expression.TrimStart(startTrim); //Odstranění nežádoucích charakterů ze začátku řetězce
            expression = expression.TrimEnd(')'); //Odstranění nežádoucích charakterů na konci řetězce
            string[] values = expression.Split(','); //Rozdělení na X část a N část
            if (values.Length != 2)
                return null;
            if ((x = Eval(values[0])) == double.NaN)
            {
                return null;
            }

            if ((n = Eval(values[1])) == double.NaN)
            {
                return null;
            }

            double result = Math.Round(Math.Pow(x, 1.0 / n), 6); 
            if (double.IsNaN(result))
                return null;
            else
                return result.ToString().Replace(',', '.');
        }

        /**
         * @brief Funkce pro výpočet n-té mocniny čísla x
         * 
         * @param expression Výraz ve tvaru x^n, který má být vyhodnocen
         * @return Výsledek výrazu, null v případě chyby výpočtu nebo tvaru
         */
        public string N_power(string expression)
        {
            expression = expression.Replace(" ", String.Empty); // Odstranění bílých znaků

            double x = 0;
            double n = 0;

            string[] values = expression.Split('^'); //Rozdělení na X část a N část
            if (values.Length != 2)
                return null;
            if((x = Eval(values[0])) == double.NaN)
            {
                return null;
            }

            if ((n = Eval(values[1])) == double.NaN)
            {
                return null;
            }


            double result = Math.Round(Math.Pow(x, n), 6);
            if (double.IsNaN(result))
                return null;
            else
                return result.ToString().Replace(',', '.');
        }

        /**
         * @brief Funkce pro výpočet faktoriálu
         * 
         * @param expression Výraz ve tvaru x!, který má být vyhodnocen
         * @return Výsledek výrazu, null v případě chyby výpočtu nebo tvaru
         */
        public string Factorial(string expression)
        {
            expression = expression.Replace(" ", String.Empty); // Odstranění bílých znaků

            double numDouble = Eval(expression.TrimEnd('!'));
            ulong num = 1;
            if (Math.Abs(numDouble % 1) <= Double.Epsilon * 1000 && numDouble >= 0)
            {
                num = Convert.ToUInt64(numDouble);
            }
            else
                return null;

            if (num < 0)
            {
                return null;
            }
            ulong result = 1;
            for (; num > 0; num--)
            {
                if(ulong.MaxValue / num < result)
                {
                    return null;
                }
                result *= num;
            }


            return result.ToString();
        }

        /**
         * @brief Funkce pro získání krajního indexu podvýrazu
         * 
         * @param expression Textový výraz
         * @param starterPos Počátešční index, od kterého se prochází výrazem
         * @param forward Určuje směr procházení (true = směr dopředu / false = směr dozadu)
         * @return Krajní index výrazu v požadovaném směru
         */
        private int GetSubstringPos(string expression, int starterPos, bool forward)
        {

            char[] seek = new char[2];
            char[] interrupt = new char[] { '+', '-', '*', '/', '^' };
            int step = 0;
            int char_counter = 0;


            if (forward)
            {
                seek[0] = '(';
                seek[1] = ')';
                step = 1;
            }
            else
            {
                seek[0] = ')';
                seek[1] = '(';
                step = -1;
            }


            for (int i = starterPos; i >= 0 && i < expression.Length; i += step)
            {
                if (char_counter == 0)
                {
                    foreach (char elem in interrupt)
                    {
                        if (expression[i] == elem)
                        {
                            return i - step;
                        }
                    }
                }
                if (expression[i] == seek[0])
                {
                    char_counter++;
                }
                if (expression[i] == seek[1])
                {
                    if (char_counter != 0)
                    {
                        char_counter--;
                    }

                    if (char_counter == 0)
                    {
                        if(forward == false && i > 0)
                        {
                            if (expression[i - 1] == 'f')
                                return i - 1;
                            if (i >= 3)
                            {
                                string sub = expression.Substring(i - 3, 4);
                                if (sub == "sin(" || sub == "tan(" || sub == "cos(" || sub == "log(")
                                    return i - 3;
                            }
                        }
                        return i;
                    }
                }
            }

            if (step == 1)
            {
                return expression.Length - 1;
            }
            else
            {
                return 0;
            }

        }

        /**
         * @brief Funkce pro odstranění přebytečných závorek, které obklopují výraz
         * 
         * @param expression Textový výraz
         * @return Výraz bez okrajových závorek
         */
        public string RemoveNotNecessaryBrackets(string expression)
        {
            if (expression.Length <= 2)
            {
                return expression;
            }
            int count = 0;
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '(')
                    count++;
                if (expression[i] == ')')
                {
                    if (count > 0)
                        count--;
                    else
                        return expression;
                }
            }
            if (count == 0 && expression[expression.Length - 1] == ')' && expression[0] == '(')
            {
                return expression.Substring(1, expression.Length - 2);
            }
            else
                return expression;
        }

        /**
         * @brief Funkce pro výpočet sin(x)
         * 
         * @param expression Výraz ve tvaru sin(x), který má být vyhodnocen
         * @return Výsledek výrazu, null v případě chyby výpočtu nebo tvaru
         */
        public string Sin(string expression)
        {
            expression = expression.Replace(" ", String.Empty); // Odstranění bílých znaků

            double x = 0;

            char[] startTrim = new char[] { 's', 'i', 'n', '(' }; /**Znaky, které mají být odstaněny ze začátku řetězce*/

            expression = expression.TrimStart(startTrim); //Odstranění nežádoucích charakterů ze začátku řetězce
            expression = expression.TrimEnd(')'); //Odstranění nežádoucích charakterů na konci řetězce
            x = Eval(expression); /**vyhodnocené X*/

            double result = Math.Round(Math.Sin(x), 6);
            if (double.IsNaN(result))
                return null;
            else
                return result.ToString();
        }

        /**
         * @brief Funkce pro výpočet cos(x)
         * 
         * @param expression Výraz ve tvaru cos(x), který má být vyhodnocen
         * @return Výsledek výrazu, null v případě chyby výpočtu nebo tvaru
         */
        public string Cos(string expression)
        {
            expression = expression.Replace(" ", String.Empty); // Odstranění bílých znaků

            double x = 0;

            char[] startTrim = new char[] { 'c', 'o', 's', '(' }; /**Znaky, které mají být odstaněny ze začátku řetězce*/

            expression = expression.TrimStart(startTrim); //Odstranění nežádoucích charakterů ze začátku řetězce
            expression = expression.TrimEnd(')'); //Odstranění nežádoucích charakterů na konci řetězce
            x = Eval(expression); /**vyhodnocené X*/

            double result = Math.Round(Math.Cos(x), 6);
            if (double.IsNaN(result))
                return null;
            else
                return result.ToString();
        }

        /**
         * @brief Funkce pro výpočet tan(x)
         * 
         * @param expression Výraz ve tvaru tan(x), který má být vyhodnocen
         * @return Výsledek výrazu, null v případě chyby výpočtu nebo tvaru
         */
        public string Tan(string expression)
        {
            expression = expression.Replace(" ", String.Empty); // Odstranění bílých znaků

            double x = 0;

            char[] startTrim = new char[] { 't', 'a', 'n', '(' }; /**Znaky, které mají být odstaněny ze začátku řetězce*/

            expression = expression.TrimStart(startTrim); //Odstranění nežádoucích charakterů ze začátku řetězce
            expression = expression.TrimEnd(')'); //Odstranění nežádoucích charakterů na konci řetězce

            x = Eval(expression); /**vyhodnocené X*/

            double result = Math.Round(Math.Tan(x), 6);
            if (double.IsNaN(result))
                return null;
            else
                return result.ToString();
        }

        public string Log(string expression)
        {
            expression = expression.Replace(" ", String.Empty); // Odstranění bílých znaků

            double x = 0;

            char[] startTrim = new char[] { 'l', 'o', 'g', '(' }; /**Znaky, které mají být odstaněny ze začátku řetězce*/

            expression = expression.TrimStart(startTrim); //Odstranění nežádoucích charakterů ze začátku řetězce
            expression = expression.TrimEnd(')'); //Odstranění nežádoucích charakterů na konci řetězce

            x = Eval(expression); /**vyhodnocené X*/


                double result = Math.Round(Math.Log10(x), 6);
            if (double.IsNaN(result))
                return null;
            else
                return result.ToString().Replace(',', '.');
        }

        /**
         * @brief Funkce pro výpočet aritmetického průměru čísel
         * 
         * @param numbers Čísla, ze kterých má být vypočítán průměr
         * @return Aritmetický průměr
         */
        public double ArithmeticMean(double[] numbers)
        {
            double res = 0.0;
            foreach (double num in numbers)
            {
                res += num;
            }

            return res / numbers.Length;
        }

    }
}
/*** Konec souboru Evaluation.cs ***/

