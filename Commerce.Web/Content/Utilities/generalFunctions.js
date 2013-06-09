var CopyPropertiesFromKo = function(from, target) { 
    target = target || {};
    for(var propertyName in from) {
        target[propertyName] = (typeof from[propertyName] == "function") ? from[propertyName]() : from[propertyName];
    }
    return target;
}

String.prototype.koTrunc = function(n, useWordBoundary){
    var toLong = 
        this.length > n, s_ = toLong ? this.substr(0,n-1) : this;
        s_ = useWordBoundary && toLong ? s_.substr(0,s_.lastIndexOf(' ')) : s_;
    return toLong ? s_ + '...' : s_;
}

Array.prototype.firstOrNull = function(lambda) {
	for (var arrayIndex = 0; arrayIndex < this.length; arrayIndex += 1) {
		var element = this[arrayIndex];
		if (lambda(element) === true) {
			return element;
		}
	}
	return null;
}

Array.prototype.arrayFirstIndexOf = function(predicate, predicateOwner) {
    for (var i = 0, j = this.length; i < j; i++) {
        if (predicate.call(predicateOwner, this[i])) {
            return i;
        }
    }
    return -1;
}

Array.prototype.remove = function(from, to) {
	var rest = this.slice((to || from) + 1 || this.length);
	this.length = from < 0 ? this.length + from : from;
	return this.push.apply(this, rest);
};

String.prototype.trim = function(){return this.replace(/^\s+|\s+$/g, '');};

String.prototype.ltrim = function(){return this.replace(/^\s+/,'');};

String.prototype.rtrim = function(){return this.replace(/\s+$/,'');};

String.prototype.fulltrim = function(){return this.replace(/(?:(?:^|\n)\s+|\s+(?:$|\n))/g,'').replace(/\s+/g,' ');};

var ToMoney = function(input) {
    return "$" + input.toFixed(2);
}

function namespace(namespaceString) {
    var parts = namespaceString.split('.'),
        parent = window,
        currentPart = '';

    for (var i = 0, length = parts.length; i < length; i++) {
        currentPart = parts[i];
        parent[currentPart] = parent[currentPart] || {};
        parent = parent[currentPart];
    }

    return parent;
}
