using System.Collections.Generic;
using System.Linq;
using BKA.Units;

namespace BKA.Buffs
{
    public class SpecializationTreeNode
    {
        public Specialization Data { get; set; }
        public SpecializationTreeNode Parent { get; set; }
        public List<SpecializationTreeNode> Children { get; set; }

        public SpecializationTreeNode(Specialization data)
        {
            Data = data;
            Children = new List<SpecializationTreeNode>();
        }
    }

    public class SpecializationStep
    {
        public Specialization SelectedSpecialization;
        public List<Specialization> Siblings;
    }

    public class SpecializationTree
    {
        private SpecializationTreeNode _root;
        private SpecializationIdentifier _specializationIdentifier;

        private SpecializationTreeNode _currentNode;

        public SpecializationTree(SpecializationIdentifier specializationIdentifier)
        {
            _specializationIdentifier = specializationIdentifier;
        }

        public void FormTree(Class heroClass)
        {
            var decoratedSpecializations = heroClass.GetDecoratedSpecializations();
            var baseSpecialization = decoratedSpecializations[0];

            UploadNode(out _root, baseSpecialization);
            _currentNode = FindNode(_root, decoratedSpecializations[^1]);
        }

        public void MakeNextStep(Specialization chosenSpecialization)
        {
            var specializationTreeNode =
                _currentNode.Children.FirstOrDefault(node =>
                    node.Data.Definition.Equals(chosenSpecialization.Definition));

            _currentNode = specializationTreeNode;
        }

        public Specialization[] GetNextSpecializations(Specialization specialization)
        {
            var specializationNode = FindNode(_root, specialization);

            return specializationNode.Children.Select(node => node.Data).ToArray();
        }

        public Specialization[] GetNextSpecializations()
        {
            return _currentNode.Children.Select(node => node.Data).ToArray();
        }

        public List<SpecializationStep> GetSpecializationPath()
        {
            var result = new List<SpecializationStep>();
            var currentSpecialization = _currentNode.Data;
            var node = _currentNode.Parent;

            while (node != null)
            {
                result.Add(new SpecializationStep
                {
                    SelectedSpecialization = currentSpecialization, Siblings = node.Children
                        .Select(treeNode => treeNode.Data).ToList()
                });
                currentSpecialization = node.Data;
                node = node.Parent;
            }

            result.Reverse();

            return result;
        }

        private void UploadNode(out SpecializationTreeNode parentNode, Specialization specialization)
        {
            parentNode = new SpecializationTreeNode(specialization);
            var specializationSequence = _specializationIdentifier.IdentifySequence(specialization);

            if (specializationSequence == null) return;

            foreach (var specializationDefinition in specializationSequence.ToSpecializations)
            {
                var childSpecialization = new Specialization(specializationDefinition);
                UploadNode(out var childSpecializationTreeNode, childSpecialization);

                childSpecializationTreeNode.Parent = parentNode;
                parentNode.Children.Add(childSpecializationTreeNode);
            }
        }

        private SpecializationTreeNode FindNode(SpecializationTreeNode startNode, Specialization specialization)
        {
            return startNode.Data.Definition.Equals(specialization.Definition)
                ? startNode
                : startNode.Children.Select(specializationTreeNode => FindNode(specializationTreeNode, specialization))
                    .FirstOrDefault(node => node != null);
        }
    }
}