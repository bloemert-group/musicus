import React from 'react';
import { Route } from 'react-router';

import MusicusPage from './pages/MusicusPage.jsx';

export default (
	<Route path="/" component={MusicusPage}>
		<Route path="/musicus" component={MusicusPage} />
	</Route>
)