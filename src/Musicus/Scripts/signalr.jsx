import * as generalActions from './actions/generalActions.jsx';

export function signalRStart(store, callback) {

	const signalR = require("@aspnet/signalr");

	let connection = new signalR.HubConnectionBuilder().withUrl('/musicushub').build();

	connection.on('SetVolume', data => {
		store.dispatch(generalActions.setVolumeLevel(data));
	});

	connection.on('SetStatus', data => {
		store.dispatch(generalActions.setTrack(data));
	});

	connection.on('SetQueue', data => {
		store.dispatch(generalActions.setQueue(data));
	});

	connection.start();

	//connection.OnDisconnected(function () {
	//	setTimeout(function () {
	//		connection.start();
	//	}, 5000);
	//});

	callback();
}