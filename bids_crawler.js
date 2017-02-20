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
	var lot = $(info[1]).text().trim();

	if (cars[lot] == undefined) {
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
	
	localStorage.setItem("cars", JSON.stringify(cars));
}, 1000);