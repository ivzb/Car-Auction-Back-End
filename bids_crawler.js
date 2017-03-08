// C:\Users\izahariev\Documents\twork\daniauto data\bids\exec\March\2nd\

// output bids
clear();
var output = [];
for (var key in cars) {
   output.push(cars[key]);
}
JSON.stringify(output);


var cars = {};

setInterval(function() {
	var currentCar = {};
	var info = $('.auction-detail-lot-details').find('tr').find('div.ng-binding');
	var lot = $(info[1]).text().trim().substr(5);

	if (cars[lot] == undefined) {
		currentCar['lot'] = lot;
		currentCar['bids'] = [];
		cars[lot] = currentCar;
		
	} else {
		// update bids
		var currentBids = [];
		$('.auction-detail-lot-bidding-current-lot .largeBidHistory p').each(function(index) {
			currentBids.push($(this).text());
		});
		cars[lot]['bids'] = currentBids;
	}
}, 1000);