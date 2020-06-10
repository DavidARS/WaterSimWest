var AssessmentText;
d3.csv('Scripts/Custom/Assessment/AssessmentText' + (LOAD_IPAD ? '_Ipad' : '') + '.csv', function (data) {
    AssessmentText = data;
});

var AssessmentFields = ['SAI_P', 'RNDR_P', 'ASI_P'];
var AssessmentFieldRanges = {
    'SAI_P': [69, 61, 51, 0],
    'RNDR_P': [98, 90, 30, 0],
    'ASI_P': [81, 71, 61, 0]
}
var AssessmentFieldPhrases = {
    'SAI_P': ['Can you make it better?', 'Try some more policies!', 'Interesting results!', 'Yikes, you can fix this!'],
    'RNDR_P': ['well balanced.', 'almost balanced.', 'not balanced!', 'crazy!'],
    'ASI_P': ['very high!', 'high.', 'challenged.', 'threatened!']
}
var AssessmentFieldBasePhrases = {
    'RNDR_P': 'Supplies and demands are',
    'ASI_P': 'The regions\' sustainability is'
}