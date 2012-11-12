using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace RadaCode.Web.Data.Entities
{
    public class SoftwareProject: IdableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string Name_En { get; set; }
        public string Description_En { get; set; }

        public virtual Customer Customer { get; set; }
        public DateTime? DateStarted { get; set; }
        public DateTime DateFinished { get; set; }
        public string WebSiteUrl { get; set; }
        public int CurrentUsersCount { get; set; }
        public int ROIpercentage { get; set; }
        public bool IsCloudConnected { get; set; }
        public string ProjectDescriptionMarkup { get; set; }

        public string ProjectDescriptionMarkup_En { get; set; }

        public virtual IList<string> SpecialFeatures
        {
            get
            {

                return _SpecialFeatures;

            }
            set
            {
                _SpecialFeatures = value;
            }
        }

        private IList<string> _SpecialFeatures;

        public string SpecialFeaturesSerialized
        {
            get
            {
                return String.Join(";", _SpecialFeatures);
            }
            set
            {
                if (value != null) _SpecialFeatures = value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList(); else
                    _SpecialFeatures = new List<string>();
            }
        }

        public virtual IList<string> SpecialFeatures_En
        {
            get
            {

                return _SpecialFeatures_En;

            }
            set
            {
                _SpecialFeatures_En = value;
            }
        }

        private IList<string> _SpecialFeatures_En;

        public string SpecialFeaturesSerialized_En
        {
            get
            {
                return String.Join(";", _SpecialFeatures_En);
            }
            set
            {
                if (value != null) _SpecialFeatures_En = value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                else
                    _SpecialFeatures_En = new List<string>();
            }
        }

        public virtual IList<string> TechnologiesUsed
        {
            get
            {

                return _TechnologiesUsed;

            }
            set
            {
                _TechnologiesUsed = value;
            }
        }

        private IList<string> _TechnologiesUsed;

        public string TechnologiesUsedSerialized
        {
            get
            {
                return String.Join(";", _TechnologiesUsed);
            }
            set
            {
                if (value != null) _TechnologiesUsed = value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList(); else 
                    _TechnologiesUsed = new List<string>();
            }
        }

        public Int64 ProjectEstimateTicks { get; set; }

        [NotMapped]
        public TimeSpan ProjectEstimate
        {
            get { return TimeSpan.FromTicks(ProjectEstimateTicks); }
            set { ProjectEstimateTicks = value.Ticks; }
        }

        public TimeSpan ProjectActualCompletionSpan
        {
            get { return DateFinished - DateStarted.Value; }
        }
    }

    public class WebDevelopmentProject: SoftwareProject {}

    public class DistributedDevelopmentProject: SoftwareProject {}

    public class MobileDevelopmentProject: SoftwareProject
    {
        public virtual IList<string> PlatformsSupported
        {
            get
            {

                return _PlatformsSupported;

            }
            set
            {
                _PlatformsSupported = value;
            }
        }

        private IList<string> _PlatformsSupported;

        public string PlatformsSupportedSerialized
        {
            get
            {
                return String.Join(";", _PlatformsSupported);
            }
            set
            {
                if (value != null) _PlatformsSupported = value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList(); else
                    _PlatformsSupported = new List<string>();
            }
        }


    }
}
