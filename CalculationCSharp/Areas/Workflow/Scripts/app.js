//
// Define the 'app' module.
//
angular.module('app', ['flowChart', 'ui.bootstrap'])

//
// Simple service to create a prompt.
//
.factory('prompt', function () {

	/* Uncomment the following to test that the prompt service is working as expected.
	return function () {
		return "Test!";
	}
	*/

	// Return the browsers prompt function.
	return prompt;
})

//
// Application controller.
//
.controller('AppCtrl', ['$scope', 'prompt', function AppCtrl ($scope, prompt, $uibModal) {

	//
	// Code for the delete key.
	//
	var deleteKeyCode = 46;

	//
	// Code for control key.
	//
	var ctrlKeyCode = 17;

	//
	// Set to true when the ctrl key is down.
	//
	var ctrlDown = false;

	//
	// Code for A key.
	//
	var aKeyCode = 65;

	//
	// Code for esc key.
	//
	var escKeyCode = 27;

	//
	// Selects the next node id.
	//
	var nextNodeID = 10;

	//
	// Setup the data-model for the chart.
	//
	var chartDataModel = {

		nodes: [
			{
				name: "Example Node 1",
				id: 0,
				x: 1,
				y: 1,
				width: 200,
				inputConnectors: [
					{
						name: "A",
					},
				],
				outputConnectors: [
					{
						name: "A",
					},
				],
			},

			{
				name: "Example Node 2",
				id: 1,
				x: 400,
				y: 200,
				width: 200,
				inputConnectors: [
					{
						name: "A",
					},
				],
				outputConnectors: [
					{
						name: "A",
					},
				],
			},

		],

		connections: [

		]
	};

	//
	// Event handler for key-down on the flowchart.
	//
	$scope.keyDown = function (evt) {

		if (evt.keyCode === ctrlKeyCode) {

			ctrlDown = true;
			evt.stopPropagation();
			evt.preventDefault();
		}
	};

	//
	// Event handler for key-up on the flowchart.
	//
	$scope.keyUp = function (evt) {

		if (evt.keyCode === deleteKeyCode) {
			//
			// Delete key.
			//
			$scope.chartViewModel.deleteSelected();
		}

		if (evt.keyCode == aKeyCode && ctrlDown) {
			// 
			// Ctrl + A
			//
			$scope.chartViewModel.selectAll();
		}

		if (evt.keyCode == escKeyCode) {
			// Escape.
			$scope.chartViewModel.deselectAll();
		}

		if (evt.keyCode === ctrlKeyCode) {
			ctrlDown = false;

			evt.stopPropagation();
			evt.preventDefault();
		}
	};

	//
	// Add a new node to the chart.
	//
	$scope.addNewNode = function () {

		var nodeName = prompt("Enter a node name:", "New node");
		if (!nodeName) {
			return;
		}

		//
		// Template for a new node.
		//
		var newNodeDataModel = {
			name: nodeName,
			id: nextNodeID++,
			x: 0,
			y: 0,
			width: 200,
			inputConnectors: [
				{
					name: "X"
				}
			],
			outputConnectors: [ 
				{
					name: "1"
				}
			],
		};

		$scope.chartViewModel.addNode(newNodeDataModel);
	};

	//
	// Add an input connector to selected nodes.
	//
	$scope.addNewInputConnector = function () {
		var connectorName = prompt("Enter a connector name:", "New connector");
		if (!connectorName) {
			return;
		}

		var selectedNodes = $scope.chartViewModel.getSelectedNodes();
		for (var i = 0; i < selectedNodes.length; ++i) {
			var node = selectedNodes[i];
			node.addInputConnector({
				name: connectorName,
			});
		}
	};

	//
	// Add an output connector to selected nodes.
	//
	$scope.addNewOutputConnector = function () {
		var connectorName = prompt("Enter a connector name:", "New connector");
		if (!connectorName) {
			return;
		}

		var selectedNodes = $scope.chartViewModel.getSelectedNodes();
		for (var i = 0; i < selectedNodes.length; ++i) {
			var node = selectedNodes[i];
			node.addOutputConnector({
				name: connectorName,
			});
		}
	};

	//
	// Delete selected nodes and connections.
	//
	$scope.deleteSelected = function () {

		$scope.chartViewModel.deleteSelected();
	};

	//
	// Create the view-model for the chart and attach to the scope.
	//
	$scope.chartViewModel = new flowchart.ChartViewModel(chartDataModel);

 

}])
.controller('stageCtrl', function ($scope, $uibModalInstance, Name, Checklist ) {
    $scope.Name = Name;
    $scope.Comments = Checklist;
    
    $scope.removeItem = function (index) {
        $scope.Tasks.splice(index, 1);
    },

    $scope.btn_add = function () {

        if ($scope.Comments == null) {
            $scope.Comments = [];
        }

        if ($scope.txtcomment != '') {
            $scope.Comments.push({
                CommentName: $scope.txtcomment
            });
            $scope.txtcomment = "";
        }
    }

    $scope.remItem = function ($index) {
        $scope.Comments.splice($index, 1);
    }

    //Click OK
    $scope.ok = function () {

        $scope.selected = {
            Name: $scope.Name,
            Checklist: $scope.Comments,
        };

        $uibModalInstance.close($scope.selected);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };


});
;