// output bids
clear();
var output = [];
for (var key in cars) {
   output.push(cars[key]);
}
JSON.stringify(output);


// handle bids
var cars = {};

setInterval(function() {
	var currentCar = {};
	var info = $('.auction-detail-lot-details').find('tr').find('div.ng-binding');
	var model = $(info[0]).text().trim();

	if (cars[model] == undefined) {
		currentCar['model'] = model;
		currentCar['lot'] = $(info[1]).text().trim();
		currentCar['category'] = $(info[2]).text().trim();
		currentCar['damage'] = $(info[3]).text().trim();
		currentCar['engineType'] = $(info[12]).text().trim();
		currentCar['bids'] = [];
		
		var currentImages = [];
		$('.image-carousel-main-images').find('img').each(function(index) {
			currentImages.push($(this).attr('src'));
		});
		currentCar['images'] = currentImages;
		
		cars[model] = currentCar;
	} else {
		// update bids
		var currentBids = [];
		$('.auction-detail-lot-bidding-current-lot .largeBidHistory p').each(function(index) {
			currentBids.push($(this).text());
		});
		cars[model]['bids'] = currentBids;
	}
}, 1000);