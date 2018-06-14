/* eslint-disable no-unused-vars */

require('../Styles/main.scss');
require('../Styles/icon.scss');

import React from 'react'

import { render } from 'react-dom';
import { Provider } from 'react-redux';
import { Router, hashHistory } from 'react-router';

import routes from './routes.jsx';
import { createStore, applyMiddleware } from 'redux'

import thunkMiddleware from 'redux-thunk'
import rootReducer from './reducers/index.jsx'
import { signalRStart } from './signalr.jsx'
//import $ from 'jquery'

let store = createStore(
	rootReducer,
	applyMiddleware(
		thunkMiddleware
	)
)

Number.prototype.pad = function (size) {
	var s = String(this);
	while (s.length < (size || 2)) { s = "0" + s; }
	return s;
}

signalRStart(store, () => {
	render(
		<Provider store={store} >
			<Router history={hashHistory} routes={routes} />
		</Provider >, document.getElementById('content')
	)
});

