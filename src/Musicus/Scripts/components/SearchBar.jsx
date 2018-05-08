import React, { PropTypes } from 'react'
import { connect } from 'react-redux';
import SearchFilter from './SearchFilter.jsx'
import SearchResult from './SearchResult.jsx'
import Ink from 'react-ink';
import { setSpotifyFilter, search, addToQueue } from '../actions/generalActions.jsx'
import * as types from '../constants/Types.jsx'
import $ from 'jquery'

class SearchBar extends React.Component {
    constructor(props) {
        super(props);
        this.onSearchFilterClick = this.onSearchFilterClick.bind(this);
        this.search = this.search.bind(this);
        this.addToQueue = this.addToQueue.bind(this);
        this.onEnterKeyPress = this.onEnterKeyPress.bind(this);

    }

    onSearchFilterClick(type) {
        switch (type) {
            case types.SPOTIFY:
                this.props.setSpotifyFilter();
                break;
        }
    }

    search() {
        const searchKey = $('#tbSearch').val();
        this.props.search(searchKey);
    }

    addToQueue(trackId, description, trackLength) {        
        this.props.addToQueue(trackId, description, trackLength);
    }

    onEnterKeyPress(e) {
        if(e.key === 'Enter'){
            const searchKey = $('#tbSearch').val();
            this.props.search(searchKey); 
        }
    }


   
    render() {
        return (
            <div className="search-bar">
                <div className="search-bar-input">
                    {<input type="text" id="tbSearch" name="tbSearch" placeholder="Zoeken..." onKeyPress={this.onEnterKeyPress} />}             
                </div>
                <div className="search-bar-btn" onTouchTap={this.search} >
                    <i className='search icon'>&nbsp;</i>
                    <Ink />
                </div>
            </div>)
    }
}

SearchBar.propTypes = {
}

function mapStateToProps(state) {
    return {
			searchResult: state.musicusState.searchResult
    }
}

export default connect(mapStateToProps, { setSpotifyFilter, search, addToQueue })(SearchBar)
