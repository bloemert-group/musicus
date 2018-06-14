import * as types from '../constants/ActionTypes.jsx'

const initMusicusState = {
	searchFilter: {
    spotify: false
  },
  searchResult: [],
  currentTrack: {
    artist: '',
    track: '',
    currentPosition: 0,
    length: 0,
		albumArtwork: '',
		trackSource: '',
		url: ''
  },
  queue: [],
  isplaying: false,
  volume: 0
}

export const musicusReducer = function init(state = initMusicusState, action) {
	switch (action.type) {
		case types.SET_SPOTIFYFILTER:
			return Object.assign({}, state, {
				searchFilter: {
					spotify: action.active
				}
			});
		case types.SET_SEARCHRESULT:
			return Object.assign({}, state, {
				searchResult: action.searchResult
			})
		case types.SET_CURRENTTRACK:
			return Object.assign({}, state, {
				currentTrack: action.currentTrack
			});
		case types.SET_QUEUE:
			return Object.assign({}, state, {
				queue: action.queue
			});
		case types.SET_PLAY:
			return Object.assign({}, state, {
				isplaying: action.isplaying
			});
		case types.SET_VOLUME:
			return Object.assign({}, state, {
				volume: action.volume
			});
		default:
			return state;
	}
}
