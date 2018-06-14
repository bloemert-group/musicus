import React from 'react'
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import Ink from 'react-ink';
import * as types from '../constants/Types.jsx'
import moment from 'moment'

class QueueItem extends React.Component {
    constructor(props) {
        super(props);
    }
    render() {
        var momentLength =  moment.duration(this.props.tracklength);
        var trackLengthFormatted = `${momentLength.minutes()}:${momentLength.seconds().pad()}`;

        var classNameTitle = "queue-list-item-title";
        var classNameDuration = "queue-list-item-duration"
        if (this.props.active) {
            classNameTitle = `${classNameTitle} active`;
            classNameDuration = `${classNameDuration} active`;
        }
  
        return (
            <div className="queue-list-item">
                <div className={classNameTitle}>
                    {this.props.description}
                </div>
                <div className={classNameDuration}>
                    {trackLengthFormatted}
                </div>
            </div>)
    }
}

QueueItem.propTypes = {
    description: PropTypes.string,
    tracklength: PropTypes.number,
    active: PropTypes.bool
}

function mapStateToProps(state) {
    return {
    }
}

export default connect(mapStateToProps)(QueueItem)
