import React, { PropTypes } from 'react'
import { connect } from 'react-redux'
import Ink from 'react-ink';
import { next, play, pause } from '../actions/generalActions.jsx'
import Header from '../components/Header.jsx'
import SearchPanel from '../components/SearchPanel.jsx'
import Queue from '../components/Queue.jsx'

class OverviewPage extends React.Component {
  render() {
    return (
      <div id="app">
        <Header />  
				<div className="body-wrapper">
					<SearchPanel />
					<Queue />
        </div>
      </div>
    )
  }
}

OverviewPage.propTypes = {

}

function mapStateToProps(state) {
  return {
		trackInfo: state.musicusState.currentTrack
  }
}

export default connect(mapStateToProps,  { next, play, pause })(OverviewPage)
