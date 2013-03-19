/*

aQuery JavaScript Array Library
Copyright (C) 2012 Daniel Henry

Permission is hereby granted, free of charge, to any person obtaining a copy of this software
and associated documentation files (the "Software"), to deal in the Software without
restriction, including without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or
substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

*/
(function() {
    function aQuery(array) {
        this.all = function(predicate) {
            /// <summary>Determines whether all elements of the array satisfy a condition.</summary>
            /// <param name="predicate">A function to test each element for a condition.</param>
            if (typeof predicate !== 'function') throw 'predicate must be a function';
            for (var i = 0, ii = array.length; i < ii; ++i)
                if (!predicate(array[i]))
                    return false;
            return true;
        };
        this.any = function(predicate) {
            /// <summary>Determines whether any element of the array satisfies a condition.</summary>
            /// <param name="predicate">A function to test each element for a condition.</param>
            if (!predicate) return array.length > 0;
            else if (typeof predicate !== 'function') throw 'predicate must be a function';
            for (var i = 0, ii = array.length; i < ii; ++i)
                if (predicate(array[i]))
                    return true;
            return false;
        };
        this.average = function(selector) {
            /// <summary>Computes the average of the array of numbers that are obtained by invoking a transform function on each element of the array.</summary>
            /// <param name="selector">A transform function to apply to each element.</param>
            if (typeof selector !== 'function')
                selector = function(e) { return e; };
            return this.sum(selector) / array.length;
        };
        this.cast = function(type) {
            /// <summary>Converts the elements of the array to the specified type.</summary>
            if (type === Boolean || type === 'boolean') return this.select(function(e) { return !!e; });
            else if (type === Number || type === 'number') return this.select(function(e) { return (+e); });
            else if (type === String || type === 'string') return this.select(function(e) { return new String(e); });
            throw 'type not recognized';
        };
        this.contains = function(value, comparer) {
            /// <summary>Determines whether the array contains a specified element by using a specified comparison function.</summary>
            /// <param name="value">The value to locate in the array.</param>
            /// <param name="comparer">An function to compare values.</param>
            if (!comparer) comparer = function(a, b) { return a === b; };
            else if (typeof comparer !== 'function') throw 'comparer must be a function';
            return this.any(function(e) { return comparer(e, value); });
        };
        this.count = function(predicate) {
            /// <summary>Returns a number that represents how many elements in the array satisfy a condition.</summary>
            /// <param name="predicate">A function to test each element for a condition.</param>
            if (!predicate) return array.length;
            else if (typeof predicate !== 'function') throw 'predicate must be a function';
            var result = 0;
            this.each(function(e) { if (predicate(e)) ++result; });
            return result;
        };
        this.crossJoin = function(second, joiner) {
            /// <summary>Correlates the elements of two arrays in every possible combination.</summary>
            /// <param name="second">The array to join to the first array.</param>
            /// <param name="joiner">A function to create a result element from two matching elements.</param>
            second = new aQuery(second);
            if (!joiner) joiner = function(a, b) {
                var o = {};
                for (prop in a)
                    if (Object.prototype.hasOwnProperty.call(a, prop))
                        o[prop] = a[prop];
                for (prop in b)
                    if (Object.prototype.hasOwnProperty.call(b, prop))
                        o[prop] = b[prop];
                return o;
            };
            if (typeof joiner !== 'function') throw 'joiner must be a function';
            var a = [];
            var result = new aQuery(a);
            this.each(function(e) {
                second.each(function(f) { a.push(joiner(e, f)); });
            });
            return result;
        };
        this.distinct = function(comparer) {
            /// <summary>Returns distinct elements from the array by using a specified comparer.</summary>
            /// <param name="comparer">An function to compare values.</param>
            if (!comparer) comparer = function(a, b) { return a === b; };
            if (typeof comparer !== 'function') throw 'comparer must be a function';
            var a = [];
            var result = new aQuery(a);
            this.each(function(e) { if (!result.any(function(f) { return comparer(e, f); })) { a.push(e); } });
            return result;
        };
        this.each = function(operation) {
            /// <summary>Performs an operation on each element in the array and returns this aQuery object.</summary>
            /// <param name="operation">A function that accepts a single parameter.</param>
            if (typeof operation !== 'function') throw 'operation must be a function';
            for (var i = 0, ii = array.length; i < ii; i++)
                operation(array[i]);
            return this;
        };
        this.elementAt = function(index) {
            /// <summary>Returns the element at a specified index in the array or a default value if the index is out of range.</summary>
            /// <param name="index">The index of the element to retrieve.</param>
            index = (+index);
            if (index < 0 || index >= array.length) throw 'index is less than 0 or greater than or equal to the number of elements in the array';
            return array[index];
        };
        this.elementAtOrDefault = function(index, defaultValue) {
            /// <summary>Returns the element at a specified index in the array.</summary>
            /// <param name="index">The index of the element to retrieve.</param>
            index = (+index);
            if (index < 0 || index >= array.length) return defaultValue;
            return array[index];
        };
        this.except = function(second, comparer) {
            /// <summary>Produces the set difference of two arrays by using a comparer.</summary>
            /// <param name="second">An aray whose elements that also occur in the this array will cause those elements to be removed from the returned array.</param>
            /// <param name="comparer">A function to compare values.</param>
            second = new aQuery(second);
            if (!comparer) comparer = function(a, b) { return a === b; };
            if (typeof comparer !== 'function') throw 'comparer must be a function';
            var result = [];
            this.each(function(e) { if (!second.contains(e, comparer)) result.push(e); });
            return new aQuery(result);
        };
        this.first = function(predicate) {
            /// <summary>Returns the first element in the array that satisfies a specified condition.</summary>
            /// <param name="predicate">A function to test each element for a condition.</param>
            if (!predicate) predicate = function(e) { return true; };
            else if (typeof predicate !== 'function') throw 'predicate must be a function';
            for (var i = 0, ii = array.length; i < ii; ++i) {
                var element = array[i];
                if (predicate(element)) return element;
            }
            throw 'no elements satisfied the condition';
        };
        this.firstOrDefault = function(predicate, defaultValue) {
            /// <summary>Returns the first element of the array that satisfies a condition or a default value if no such element is found.</summary>
            /// <param name="predicate">A function to test each element for a condition.</param>
            /// <param name="defaultValue">The value to return if no element is found that satisfies the condition.</param>
            var usedPredicate = null, usedDefaultValue = null;
            for (var a = 0, aa = arguments.length; a < aa; ++a) {
                var arg = arguments[a];
                if (typeof arg === 'function' && !usedPredicate) usedPredicate = arg;
                else if (arg && !usedDefaultValue) usedDefaultValue = arg;
            }
            if (!usedPredicate) usedPredicate = function(e) { return true; };
            for (var i = 0, ii = array.length; i < ii; ++i) {
                var element = array[i];
                if (usedPredicate(element)) return element;
            }
            return usedDefaultValue;
        };
        this.groupBy = function(keySelector, comparer) {
            /// <summary>Groups the elements of the array according to a specified key selector function and compares the keys by using a specified comparer.</summary>
            /// <param name="keySelector">A function to extract the key for each element.</param>
            /// <param name="comparer">A function to compare keys.</param>
            if (typeof keySelector !== 'function') throw 'keySelector must be a function';
            if (!comparer) comparer = function(a, b) { return a === b; };
            if (typeof comparer !== 'function') throw 'comparer must be a function';
            var a = [];
            var q = new aQuery(a);
            this.each(function(e) {
                var key = keySelector(e);
                var group = q.firstOrDefault(function(g) { return comparer(g.key, key); });
                if (group === null) {
                    group = { key: key, values: [] };
                    a.push(group);
                }
                group.values.push(e);
            });
            return q.select(function(e) {
                return {
                    key: e.key,
                    values: new aQuery(e.values)
                };
            });
        };
        this.indexOf = function(predicate) {
            /// <summary>Returns the index of the first element of the array that satisfies a condition or -1 if no such element is found.</summary>
            /// <param name="predicate">A function to test each element for a condition.</param>
            if (typeof predicate !== 'function') throw 'predicate must be a function';
            for (var i = 0, ii = array.length; i < ii; ++i)
                if (predicate(array[i]))
                    return i;
            return -1;
        };
        this.intersect = function(second, comparer) {
            /// <summary>Produces the set intersection of two arrays by using a comparer.</summary>
            /// <param name="second">An array whose distinct elements that also appear in the first array will be returned.</param>
            /// <param name="comparer">A function to compare values.</param>
            second = new aQuery(second);
            if (!comparer) comparer = function(a, b) { return a === b; };
            if (typeof comparer !== 'function') throw 'comparer must be a function';
            var result = [];
            this.distinct(comparer).each(function(e) { if (second.contains(e, comparer)) result.push(e); });
            return new aQuery(result);
        };
        this.join = function(second, predicate, joiner) {
            /// <summary>Correlates the elements of two arrays based on matching keys. A specified function is used to compare keys.</summary>
            /// <param name="second">The array to join to the first array.</param>
            /// <param name="predicate">A function that determines whether an element from this array matches an element of the second array.</param>
            /// <param name="joiner">A function to create a result element from two matching elements.</param>
            second = new aQuery(second);
            if (typeof predicate !== 'function') throw 'predicate must be a function';
            if (!joiner) joiner = function(a, b) {
                var o = {};
                for (prop in a)
                    if (Object.prototype.hasOwnProperty.call(a, prop))
                        o[prop] = a[prop];
                for (prop in b)
                    if (Object.prototype.hasOwnProperty.call(b, prop))
                        o[prop] = b[prop];
                return o;
            };
            if (typeof joiner !== 'function') throw 'joiner must be a function';
            var a = [];
            var result = new aQuery(a);
            this.each(function(e) {
                second.where(function(f) { return predicate(e, f); }).each(function(f) { a.push(joiner(e, f)); });
            });
            return result;
        };
        this.last = function(predicate) {
            /// <summary>Returns the last element of the array that satisfies a specified condition.</summary>
            /// <param name="predicate">A function to test each element for a condition.</param>
            if (!predicate) predicate = function(e) { return true; };
            else if (typeof predicate !== 'function') throw 'predicate must be a function';
            for (var i = array.length - 1; i >= 0; --i) {
                var element = array[i];
                if (predicate(element)) return element;
            }
            throw 'no elements satisfied the condition';
        };
        this.lastIndexOf = function(predicate) {
            /// <summary>Returns the index of the last element of the array that satisfies a condition or -1 if no such element is found.</summary>
            /// <param name="predicate">A function to test each element for a condition.</param>
            if (typeof predicate !== 'function') throw 'predicate must be a function';
            for (var i = array.length - 1; i >= 0; --i)
                if (predicate(array[i]))
                    return i;
            return -1;
        };
        this.lastOrDefault = function(predicate, defaultValue) {
            /// <summary>Returns the last element of the array that satisfies a condition or a default value if no such element is found.</summary>
            /// <param name="predicate">A function to test each element for a condition.</param>
            /// <param name="defaultValue">The value to return if no element is found that satisfies the condition.</param>
            var usedPredicate = null, usedDefaultValue = null;
            for (var a = 0, aa = arguments.length; a < aa; ++a) {
                var arg = arguments[a];
                if (typeof arg === 'function' && !usedPredicate) usedPredicate = arg;
                else if (arg && !usedDefaultValue) usedDefaultValue = arg;
            }
            if (!usedPredicate) usedPredicate = function(e) { return true; };
            for (var i = array.length - 1; i >= 0; --i) {
                var element = array[i];
                if (usedPredicate(element)) return element;
            }
            return usedDefaultValue;
        };
        this.max = function(selector) {
            /// <summary>Invokes a transform function on each element of the array and returns the maximum value.</summary>
            /// <param name="selector">A transform function to apply to each element.</param>
            if (!selector) selector = function(e) { return e; };
            if (typeof selector !== 'function') throw 'selector must be a function';
            if (array.length === 0) throw 'array contains no elements';
            var maxValue = null;
            this.each(function(e) {
                var value = selector(e);
                if (value > maxValue || !maxValue) maxValue = value;
            });
            return maxValue;
        };
        this.min = function(selector) {
            /// <summary>Invokes a transform function on each element of the array and returns the minimum value.</summary>
            /// <param name="selector">A transform function to apply to each element.</param>
            if (!selector) selector = function(e) { return e; };
            if (typeof selector !== 'function') throw 'selector must be a function';
            if (array.length === 0) throw 'array contains no elements';
            var minValue = null;
            this.each(function(e) {
                var value = selector(e);
                if (value < minValue || !minValue) minValue = value;
            });
            return minValue;
        };
        this.ofType = function(type) {
            /// <summary>Filters the elements of the array based on a specified type.</summary>
            /// <param name="type">The type to filter the elements of the array on.</param>
            var result = [];
            if (typeof type === 'string')
                this.each(function(e) {
                    if (typeof e === type ||
                        (type === 'boolean' && e instanceof Boolean) ||
                        (type === 'null' && e === null) ||
                        (type === 'object' && e instanceof Object) ||
                        (type === 'number' && e instanceof Number) ||
                        (type === 'string' && e instanceof String) ||
                        (type === 'undefined' && e === undefined))
                        result.push(e);
                });
            else
                this.each(function(e) {
                    if ((type !== undefined && type !== null && e instanceof type) ||
                        (type === Boolean && typeof e === 'boolean') ||
                        (type === null && e === null) ||
                        (type === Object && typeof e === 'object') ||
                        (type === Number && typeof e === 'number') ||
                        (type === String && typeof e === 'string') ||
                        (type === undefined && e === undefined))
                        result.push(e);
                });
            return new aQuery(result);
        };
        this.orderBy = function(keySelector) {
            /// <summary>Sorts the elements of the array in ascending order according to a key.</summary>
            /// <param name="keySelector">A function to extract a key from an element.</param>
            if (!keySelector) keySelector = function(e) { return e; };
            if (typeof keySelector !== 'function') throw 'keySelector must be a function';
            var result = array.slice();
            result.sort(function(a, b) {
                var aVal = keySelector(a);
                var bVal = keySelector(b);
                if (aVal < bVal) return -1;
                else if (aVal > bVal) return 1;
                return 0;
            });
            return new aQuery(result);
        };
        this.orderByDescending = function(keySelector) {
            /// <summary>Sorts the elements of the array in descending order according to a key.</summary>
            /// <param name="selector">A function to extract a key from an element.</param>
            if (!keySelector) keySelector = function(e) { return e; };
            if (typeof keySelector !== 'function') throw 'keySelector must be a function';
            var result = array.slice();
            result.sort(function(a, b) {
                var aVal = keySelector(a);
                var bVal = keySelector(b);
                if (aVal < bVal) return 1;
                else if (aVal > bVal) return -1;
                return 0;
            });
            return new aQuery(result);
        };
        this.outerJoin = function(second, predicate, joiner) {
            /// <summary>Correlates the elements of two arrays based on matching keys. A specified function is used to compare keys. Also includes un-matched elements.</summary>
            /// <param name="second">The array to join to the first array.</param>
            /// <param name="predicate">A function that determines whether an element from this array matches an element of the second array.</param>
            /// <param name="joiner">A function to create a result element from two matching elements.</param>
            second = new aQuery(second);
            if (typeof predicate !== 'function') throw 'predicate must be a function';
            if (!joiner) joiner = function(a, b) {
                var o = {};
                for (prop in a)
                    if (Object.prototype.hasOwnProperty.call(a, prop))
                        o[prop] = a[prop];
                for (prop in b)
                    if (Object.prototype.hasOwnProperty.call(b, prop))
                        o[prop] = b[prop];
                return o;
            };
            if (typeof joiner !== 'function') throw 'joiner must be a function';
            var a = [];
            var result = new aQuery(a);
            this.each(function(e) {
                second.where(function(f) { return predicate(e, f); }).each(function(f) { a.push(joiner(e, f)); });
            });
            var that = this;
            this.where(function(e) { return !second.any(function(f) { return predicate(e, f); }); }).each(function(e) { a.push(joiner(e, null)); });
            second.where(function(f) { return !that.any(function(e) { return predicate(e, f); }); }).each(function(f) { a.push(joiner(null, f)); });
            return result;
        };
        this.select = function(selector) {
            /// <summary>Projects each element of the array into a new form.</summary>
            /// <param name="selector">A transform function to apply to each element.</param>
            if (typeof selector !== 'function') throw 'selector must be a function';
            var result = [];
            this.each(function(e) { result.push(selector(e)); });
            return new aQuery(result);
        };
        this.sequenceEqual = function(second, comparer) {
            /// <summary>Determines whether two arrays are equal by comparing their elements by using a specified comparer.</summary>
            /// <param name="second">An array to compare to the first sequence.</param>
            /// <param name="comparer">An function to use to compare elements.</param>
            second = new aQuery(second);
            if (!comparer) comparer = function(a, b) { return a === b; };
            if (typeof comparer !== 'function') throw 'comparer must be a function';
            var that = this;
            if (array.length !== second.count()) return false;
            else if (this.any(function(e) { return !second.contains(e); })) return false;
            else if (second.any(function(e) { return !that.contains(e); })) return false;
            return true;
        };
        this.single = function(predicate) {
            /// <summary>Returns the only element of the array that satisfies a specified condition, and throws an exception if more than one such element exists.</summary>
            /// <param name="predicate">A function to test an element for a condition.</param>
            if (!predicate) predicate = function(e) { return true; };
            else if (typeof predicate !== 'function') throw 'predicate must be a function';
            var match = null, matchesFound = 0;
            this.each(function(e) { if (predicate(e)) { match = e; ++matchesFound } });
            if (matchesFound == 0) throw 'no elements satisfied the condition';
            else if (matchesFound > 1) throw 'more than one element satisfied the condition';
            return match;
        };
        this.singleOrDefault = function(predicate, defaultValue) {
            /// <summary>Returns the only element of the array that satisfies a specified condition or a default value if no such element exists; this function throws an exception if more than one element satisfies the condition.</summary>
            /// <param name="predicate">A function to test an element for a condition.</param>
            /// <param name="defaultValue">The value to return if no element is found that satisfies the condition.</param>
            var usedPredicate = null, usedDefaultValue = null;
            for (var a = 0, aa = arguments.length; a < aa; ++a) {
                var arg = arguments[a];
                if (typeof arg === 'function' && !usedPredicate) usedPredicate = arg;
                else if (arg && !usedDefaultValue) usedDefaultValue = arg;
            }
            if (!usedPredicate) usedPredicate = function(e) { return true; };
            var match = null, matchesFound = 0;
            this.each(function(e) { if (usedPredicate(e)) { match = e; ++matchesFound } });
            if (matchesFound == 0) return usedDefaultValue;
            else if (matchesFound > 1) throw 'more than one element satisfied the condition';
            return match;
        };
        this.skip = function(count) {
            /// <summary>Bypasses a specified number of elements in the array and then returns the remaining elements.</summary>
            /// <param name="count">The number of elements to skip before returning the remaining elements.</param>
            var result = [];
            for (var i = (+count), ii = array.length; i < ii; ++i)
                result.push(array[i]);
            return new aQuery(result);
        };
        this.skipWhile = function(predicate) {
            /// <summary>Bypasses elements in the array as long as a specified condition is true and then returns the remaining elements.</summary>
            /// <param name="predicate">A function to test each element for a condition.</param>
            if (typeof predicate !== 'function') throw 'predicate must be a function';
            var result = [];
            var i = 0, ii = array.length;
            while (i < ii) {
                if (!predicate(array[i])) break;
                ++i;
            }
            while (i < ii) {
                result.push(array[i]);
                ++i;
            }
            return new aQuery(result);
        };
        this.sum = function(selector) {
            /// <summary>Computes the sum of the array of numbers that are obtained by invoking a transform function on each element of the array.</summary>
            /// <param name="selector">The transform function to apply to each element.</param>
            if (!selector) selection = function(e) { return e; };
            else if (typeof selector !== 'function') throw 'selector must be a function';
            var result = 0;
            this.each(function(e) { result += selector(e); });
            return result;
        };
        this.take = function(count) {
            /// <summary>Returns a specified number of contiguous elements from the start of the array.</summary>
            /// <param name="count">The number of elements to return.</count>
            var result = [];
            for (var i = 0, ii = count < array.length ? count : array.length; i < ii; ++i)
                result.push(array[i]);
            return new aQuery(result);
        };
        this.takeWhile = function(predicate) {
            /// <summary>Returns elements from an array as long as a specified condition is true.</summary>
            /// <param name="predicate">A function to test each element for a condition.</param>
            if (typeof predicate !== 'function') throw 'predicate must be a function';
            var result = [];
            for (var i = 0, ii = array.length; i < ii; ++i) {
                var element = array[i];
                if (!predicate(element)) break;
                result.push(element);
            }
            return new aQuery(result);
        };
        this.toArray = function() {
            /// <summary>Returns the array.</summary>
            return array.slice();
        };
        this.toString = function() {
            /// <summary>Returns the result of invoking the array's toString function.</summary>
            return array.toString();
        };
        this.union = function(second, comparer) {
            /// <summary>Produces the set union of two arrays by using a specified comparer.</summary>
            /// <param name="second">An array whose distinct elements form the second set for the union.</param>
            /// <param name="comparer">A function to compare values.</param>
            second = new aQuery(second);
            if (!comparer) comparer = function(a, b) { return a === b; };
            if (typeof comparer !== 'function') throw 'comparer must be a function';
            var a = this.distinct().toArray();
            var result = new aQuery(a);
            second.each(function(e) { if (!result.contains(e)) { a.push(e); } });
            return result;
        };
        this.where = function(predicate) {
            /// <summary>Filters the array of values based on a predicate.</summary>
            /// <param name="predicate">A function to test each element for a condition.</param>
            if (typeof predicate !== 'function') throw 'predicate must be a function';
            var result = [];
            this.each(function(e) { if (predicate(e)) result.push(e); });
            return new aQuery(result);
        };
    }
    var AQ = function create(o) {
        /// <summary>Create a new aQuery object.</summary>
        /// <param name="o">
        /// One of the following items:
        /// &#10;- An array
        /// &#10;- An aQuery object
        /// &#10;- An object with a defined toArray function
        /// &#10;- An array-like object (e.g. arguments)
        /// </param>
        if (o instanceof Array) return new aQuery(o);
        else if (o instanceof aQuery) return o;
        else if (o.toArray) create(o.toArray());
        else return new aQuery(Array.prototype.slice.call(o));
    };
    AQ.isQuery = function(o) {
        /// <summary>Determines if an object is an aQuery object.</summary>
        /// <param name="o">The object to test.</param>
        return o instanceof aQuery;
    };
    AQ.range = function(start, count) {
        /// <summary>Generates an array of integral numbers within a specified range.</summary>
        /// <param name="start">The value of the first integer in the array.</param>
        /// <param name="count">The number of sequential integers to generate.</param>
        var result = [];
        for (var i = start, ii = start + count; i <= ii; ++i)
            result.push(i);
        return new aQuery(result);
    };
    if (!Array.prototype.toQuery) Array.prototype.toQuery = function() {
        /// <summary>Converts this array into an aQuery object.</summary>
        return new aQuery(this);
    };
    if (typeof module !== 'undefined' && module.exports) module.exports = AQ;
    else if (typeof window !== 'undefined') window.AQ = AQ;
} ());