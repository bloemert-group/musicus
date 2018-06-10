import * as actionTypes from '../constants/ActionTypes.jsx'
import fetch from 'isomorphic-fetch'
import * as general from '../constants/General.jsx'
import $ from 'jquery'

function setSpotifyFilterAction(active) {
	return {
		type: actionTypes.SET_SPOTIFYFILTER,
		active
	}
}

export function setSpotifyFilter() {
	return (dispatch, getState) => {
		const active = !getState().musicusState.searchFilter.spotify;
		dispatch(setSpotifyFilterAction(active));
	}
}

function setSearchResultAction(searchResult) {
	return {
		type: actionTypes.SET_SEARCHRESULT,
		searchResult
	}
}

export function search(keyword) {
	return (dispatch, getState) => {
		const searchFilter = {
			Keyword: keyword,
			Spotify: true,
			Youtube: false
		};

		fetch(general.API_URL_SEARCH, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify(searchFilter)
		})
			.then(response => response.json())
			.then(json => {
				dispatch(setSearchResultAction(json));
			})
			.catch(() => {
				console.log('error');
			});
	}
}

export function addToQueue(trackid, artist, description, trackLength, url, source) {
	return (dispatch) => {
		const track = {
			Artist: artist,
			Description: description,
			TrackId: trackid,
			TrackLength: trackLength,
			Url: url,
			TrackSource: source
		};

		fetch(general.API_URL_ADD_TO_QUEUE, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify(track)
		})
			.then(response => response.json())
			.then(json => {
				dispatch(setQueue(json.data));
			})
			.catch(() => {
				console.log('error');
			});
	}
}

export function play(currentTrack) {
	return (dispatch) => {
		const track = {
			Artist: currentTrack.artist,
			Description: currentTrack.track,
			TrackSource: currentTrack.trackSource
		};

		fetch(general.API_URL_PLAY, {
			method: 'POST',
			headers: {
				'Content-Type' : 'application/json'
			},
			body: JSON.stringify(track)
		})
			.then(response => response.json())
			.then(json => {
				console.log(json);
			})
			.catch((e) => {
				console.log(e);
				console.log('error');
			});
	}
}

export function pause(currentTrack) {
	return (dispatch) => {
		const track = {
			Artist: currentTrack.artist,
			Description: currentTrack.track,
			TrackSource: currentTrack.trackSource
		};

		fetch(general.API_URL_PAUSE, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify(track)
		})
			.then(response => response.json())
			.then(json => {
				console.log(json);
			})
			.catch((e) => {
				console.log(e);
			});
	}
}

export function next() {
	return (dispatch) => {
		fetch(general.API_URL_NEXT, {
			method: 'POST'
		})
			.then(response => response.json())
			.then(json => {
				console.log(json);
			})
			.catch(() => {
				console.log('error');
			});
	}
}

export function setTrack(trackInfo) {
	return dispatch => {
		dispatch(setTrackInfo(trackInfo));
		dispatch(setPlaying(trackInfo.play));
	}
}

export function setQueue(queue) {
	return dispatch => {
		dispatch(setQueueInfo(queue));
	}
}

function setTrackInfo(trackInfo) {
	return {
		type: actionTypes.SET_CURRENTTRACK,
		currentTrack: {
			artist: trackInfo.artist,
			track: trackInfo.track,
			currentPosition: trackInfo.current,
			length: trackInfo.length,
			albumArtwork: trackInfo.albumArtWork,
			trackSource: trackInfo.trackSource
		}
	};
}

function setPlaying(isplaying) {
	return {
		type: actionTypes.SET_PLAY,
		isplaying
	}
}

function setQueueInfo(queue) {
	return {
		type: actionTypes.SET_QUEUE,
		queue
	}
}

export function setVolume(percentage) {
	return (dispatch) => {
		fetch(`${general.API_URL_VOLUME}/${percentage}`, {
			method: 'GET',
			headers: {
				'Content-Type': 'application/json'
			}
		})
			.then(response => response.json())
			.then(json => {
				console.log(json);
			})
			.catch(() => {
				console.log('error');
			});
	}
}

export function setVolumeLevel(percentage) {
	return {
		type: actionTypes.SET_VOLUME,
		volume: percentage
	}
}
