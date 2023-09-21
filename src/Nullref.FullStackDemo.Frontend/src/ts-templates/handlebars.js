
module.exports = function (handlebars) {
    handlebars.registerHelper('isdefined', function (value) {
        return value !== undefined;
    });

    handlebars.registerHelper('isdefinedString', function (value, type) {
        return value !== undefined && type.toString().indexOf('string') >=0;
    });

    handlebars.registerHelper('isdefinedNumberBool', function (value, type) {
        return value !== undefined && (type.toString().indexOf('boolean') >= 0 || type.toString().indexOf('number') >= 0);
    });

    handlebars.registerHelper('isdefinedEnum', function (value, type) {
        return value !== undefined && type.toString() !== 'string' && (typeof value === 'string');
    });
};
