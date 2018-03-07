/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package klunko.zp3jv.s08;

import java.util.Arrays;
import java.util.Deque;
import java.util.HashMap;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;
import java.util.NoSuchElementException;

/**
 * Jednoduchý interpret výrazů v postfixové notaci
 *
 * @author Michael Klunko
 * @version 1.0
 */
public class RPN {

    public static final String T = "#t";
    public static final String F = "#f";
    public static final String comps[] = {"<", ">", "=", "!=", "<=", ">="};
    public static final char operators[] = {'+', '-', '*', '/'};


    /**
     * Vyhodnotí výraz v postfixové notaci
     *
     * @param expr - Výraz k vyhodnocení
     * @return Číslo, #t, nebo #f
     * @throws IllegalArgumentException - hodnoty a, b jsou vzájemně
     * nesrovnatelné
     * @throws UnsupportedOperationException - comparator není mezi
     * podporovanými operátory
     * @throws StringIndexOutOfBoundsException - Přebývající bílý znak
     * @throws NoSuchElementException - nedostatek operandů
     */
    static Object rpnCalc(String expr)
            throws IllegalArgumentException,
            UnsupportedOperationException, NoSuchElementException,
            ArithmeticException {
        return rpnCalc(expr, new HashMap<String, Object>());
    }

    /**
     * Vyhodnotí výraz v postfixové notaci
     *
     * @param expr - Výraz k vyhodnocení
     * @param variables - HashMap z hodnoty proměnnych
     * @return Číslo, #t, nebo #f
     * @throws IllegalArgumentException - hodnoty a, b jsou vzájemně
     * nesrovnatelné
     * @throws UnsupportedOperationException - comparator není mezi
     * podporovanými operátory
     * @throws StringIndexOutOfBoundsException - Přebývající bílý znak
     * @throws NoSuchElementException - nedostatek operandů
     */
    static Object rpnCalc(String expr, Map<String, Object> variables)
            throws IllegalArgumentException,
            UnsupportedOperationException, NoSuchElementException,
            ArithmeticException {
        
        /**
        *Jestlí bude provedená operace z čisly typu float, nastavime hodnotu
        *proměnné na true
        **/
        boolean returnFloat = false;
        
        Deque<Object> stack = new LinkedList<>();

        List<String> exprSplit = Arrays.asList(expr.split("\\s+"));
        for (String symbol : exprSplit) {
            if (isNumber(symbol)) {
                if (isFloat(symbol)) {
                    returnFloat = true;
                }
                stack.push(Float.parseFloat(symbol));
            } // Pravdivostní hodnota
            else if (symbol.equals(T) || symbol.equals(F)) {
                stack.push(symbol);
            } // Proměnná
            else if (variables.containsKey(symbol)) {
                stack.push(variables.get(symbol));
            } //Operator
            else {
                int i = exprSplit.indexOf(symbol);
                if ((exprSplit.size() > i + 1)
                        && (exprSplit.get(i + 1).equals("?"))) {
                    stack.push(symbol);
                } else if (symbol.equals("?")) {
                    String operation = stack.pop().toString();
                    float a = (Float) stack.pop();
                    float b = (Float) stack.pop();
                    float c = (Float) stack.pop();
                    float d = (Float) stack.pop();
                    stack.push(compare(operation, b, a).equals(T) ? c : d);
                } // Operátor porovnání
                else if (Arrays.asList(comps).contains(symbol)) {
                    Object a = stack.pop();
                    Object b = stack.pop();
                    stack.push(compare(symbol, b, a));

                } // Aritmetický operátor
                else if (new String(operators).contains(symbol)) {
                    char operator = symbol.charAt(0);
                    float a = (Float) stack.pop();
                    float b = (Float) stack.pop();
                    stack.push(execute(operator, b, a));
                } // Neznámý operátor
                else {
                    throw new UnsupportedOperationException();
                }
            }
        }

        Object r = stack.pop();
        if (r instanceof String) {
            return r;
        } else if (returnFloat) {
            return r;
        } else {
            return ((Float) r).intValue();
        }
    }

    /**
     * Porovná dvě hodnoty
     *
     * @param comp - Operátor porovnání
     * @see String comps
     * @param a
     * @param b
     * @return #f nebo #t
     * @throws IllegalArgumentException - hodnoty a, b jsou vzájemně
     * nesrovnatelné
     * @throws UnsupportedOperationException - comparator není mezi
     * podporovanými operátory
     */
    private static String compare(String comp, Object a, Object b)
            throws IllegalArgumentException, UnsupportedOperationException {
        // Číslo vs nečíselná hodnota
        if ((isNumber(a.toString())) ^ (isNumber(b.toString()))) {
            throw new IllegalArgumentException();
        } // Číslo vs číslo
        else if ((isNumber(a.toString())) && (isNumber(b.toString()))) {
            return compareArgs(comp, (Float) a, (Float) b) ? T : F;
        } // Nečíselná hodnota vs nečíselná hodnota
        else {
            return compareArgs(comp, a.toString(), b.toString()) ? T : F;
        }
    }

    /**
     * Zjistí, jestli argument je možný převest na číslo
     *
     * @param s
     * @return boolean
     */
    private static boolean isNumber(String s) {
        return isFloat(s) || isInteger(s);
    }

    /**
     * Zjistí, jestli argument je možný převest na desetinné číslo
     *
     * @param s - String
     * @return boolean
     */
    private static boolean isFloat(String s) {
        try {
            Float.parseFloat(s);
        } catch (NumberFormatException e) {
            return false;
        }
        return isInteger(s) ? false : true;
    }

    /**
     * Zjistí, jestli argument je možný převest na celé číslo
     *
     * @param s - String
     * @return boolean
     */
    private static boolean isInteger(String s) {
        try {
            Integer.parseInt(s);
        } catch (NumberFormatException e) {
            return false;
        }
        return true;
    }

    /**
     * Provede aritmetickou operaci se zadanými operandy
     *
     * @param operation - Aritmetický operátor (podporované: '+', '-', '*', '/')
     * @param a - Číslo
     * @param b - Číslo
     * @return Desetinné číslo
     * @throws ArithmeticException - Dělení nulou
     * @throws UnsupportedOperationException - operation není mezi podporovanými
     * operátory
     */
    private static float execute(char operation, float a, float b)
            throws UnsupportedOperationException,
            ArithmeticException {
        if ((operation == '/') && (b == 0.0)) {
            throw new ArithmeticException("divide by zero");
        }
        switch (operation) {
            case '+':
                return a + b;
            case '-':
                return a - b;
            case '*':
                return a * b;
            case '/':
                return a / b;
        }
        throw new UnsupportedOperationException();
    }

    /**
     * Porovnává argumenty
     *
     * @param a - Číslo
     * @param b - Číslo
     * @param comp - "=", "!=", ">", "<", ">=", "<="
     * @return boolean
     * @throws UnsupportedOperationException - comparator není podporovan
     */
    private static boolean compareArgs(String comp, float a, float b)
            throws UnsupportedOperationException {
        switch (comp) {
            case "=":
                return a == b;
            case "!=":
                return a != b;
            case ">":
                return a > b;
            case "<":
                return a < b;
            case ">=":
                return a >= b;
            case "<=":
                return a <= b;
            default:
                break;
        }
        throw new UnsupportedOperationException();
    }

    /**
     * Porovná dva pravdivostní argumenty reprezentované pomocí #f, #t
     *
     * @param comparator - Symbol porovnání (podporované: "<", ">", "=", "!=",
     * "<=", ">=")
     * @param a - Číslo
     * @param b - Číslo
     * @return boolean
     * @throws UnsupportedOperationException - comparator není mezi
     * podporovanými operátory
     */
    private static boolean compareArgs(String comparator, String a,
            String b) throws UnsupportedOperationException {
        switch (comparator) {
            case "=":
                return a.equals(b);
            case "!=":
                return !a.equals(b);
            case ">":
                return a.equals(T) && b.equals(F);
            case "<":
                return a.equals(F) && b.equals(T);
            case ">=":
                return a.equals(b) || (a.equals(T) && b.equals(F));
            case "<=":
                return a.equals(b) || (a.equals(F) && b.equals(T));
            default:
                break;
        }
        throw new UnsupportedOperationException();
    }
}
