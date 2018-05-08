import React from 'react'
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import Ink from 'react-ink';
import * as types from '../constants/Types.jsx'
import moment from 'moment'

class SearchResult extends React.Component {
    constructor(props) {
        super(props);
        this.click = this.click.bind(this);
    }

    click(e) {
        e.preventDefault();
        this.props.onclick(this.props.trackid, this.props.description, this.props.tracklength);
    }

    render() {

        var momentLength =  moment.duration(this.props.tracklength * 1000);

        var trackLengthFormatted = `${momentLength.minutes()}:${momentLength.seconds().pad()}`;
        var playlistTracks = `Tracks: ${this.props.trackcount}`;

        var durationTrackCount = (<div className="search-list-item-duration">
        {trackLengthFormatted}
    </div>);
        if (this.props.searchtype === 1) {
            var durationTrackCount = (<div className="search-list-item-tracks">
            {playlistTracks}
        </div>);
        }

        return (
            <div className="search-list-item" onTouchTap={this.click}>
                <div className="search-list-item-title">
                    {this.props.description}
                </div>
                {durationTrackCount}
                <div className="search-list-item-add">
                    <i className="plus icon"></i>
                </div>
               
                    <Ink />
            </div>)
    }
}

SearchResult.propTypes = {
    trackid: PropTypes.string,
    description: PropTypes.string,
    onclick: PropTypes.func,
    tracklength: PropTypes.number,
    searchtype: PropTypes.number,
    trackcount: PropTypes.number
}

function mapStateToProps(state) {
    return {
    }
}

export default connect(mapStateToProps)(SearchResult)
