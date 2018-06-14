import React, { PropTypes } from 'react'
import { connect } from 'react-redux';
import Ink from 'react-ink';
import { setSpotifyFilter, search, addToQueue } from '../actions/generalActions.jsx'
import * as types from '../constants/Types.jsx'
import $ from 'jquery'

class SearchAlbumResult extends React.Component {
    constructor(props) {
        super(props);
    }

   
    render() {
        return (
            <div className="search-album-result">
                <div className="album-item"></div>
                <div className="album-item"></div>
                <div className="album-item"></div>
                <div className="album-item"></div>
                <div className="album-item"></div>
                <div className="album-item"></div>
                <div className="album-item"></div>
                <div className="album-item"></div>
                <div className="album-item"></div>
                <div className="album-item"></div>
                <div className="album-item"></div>
                <div className="album-item"></div>
                <div className="album-item"></div>
                <div className="album-item"></div>
                <div className="album-item"></div>
                

            </div>
            )
    }
}

SearchAlbumResult.propTypes = {
}

function mapStateToProps(state) {
    return {
			searchResult: state.musicusState.searchResult
    }
}

export default connect(mapStateToProps, { setSpotifyFilter, search, addToQueue })(SearchAlbumResult)
